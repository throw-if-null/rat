using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rat.Api.Auth;
using Rat.Api.Test.Auth;
using Rat.Api.Test.Mocks;
using Rat.DataAccess;
using Snapshooter;

namespace Rat.Api.Test
{
	internal class TestApplication : WebApplicationFactory<Program>
	{
		public TestApplication() : base()
		{
		}

		protected override void ConfigureClient(HttpClient client)
		{
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
		}

		protected override IHost CreateHost(IHostBuilder builder)
		{
			builder.UseEnvironment("Development");

			builder.ConfigureAppConfiguration(configurationBuilder =>
			{
				configurationBuilder
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
					.AddEnvironmentVariables();
			});

			builder.ConfigureServices(services =>
			{
				services.AddHttpClient();

				services.AddSingleton<IUserProvider, TestUserProvider>();

				services
					.AddAuthentication("Test")
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

				using var provider = services.BuildServiceProvider();
				using var scope = provider.CreateScope();

				var scopedServices = scope.ServiceProvider;
				var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
				var logger = scopedServices.GetRequiredService<ILogger<TestApplication>>();

				try
				{
					using var connection = connectionFactory.CreateConnection();

					connection.Open();

					if (connection.DatabaseExists("RatDb"))
						connection.DropDatabase("RatDb");

					connection.CreateDatabase("RatDb");
					connection.CreateTable("RatDb", "ProjectType.sql");
					connection.CreateTable("RatDb", "Project.sql");
					connection.CreateTable("RatDb", "ConfigurationType.sql");
					connection.CreateTable("RatDb", "ConfigurationRoot.sql");
					connection.CreateTable("RatDb", "ConfigurationEntry.sql");
					connection.CreateTable("RatDb", "Member.sql");
					connection.CreateTable("RatDb", "MemberProject.sql");

					AddProjectType(connection, "RatDb", "js");
					AddProjectType(connection, "RatDb", "csharp");
					AddUser(connection, "RatDb");
				}
				catch (Exception ex)
				{

					throw;
				}
			});

			return base.CreateHost(builder);
		}

		private static void AddProjectType(SqlConnection connection, string database, string name)
		{
			var getCommand = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name = @Name", new { Name = name });
			var insertCommand = new CommandDefinition("INSERT INTO ProjectType (Name) VALUES(@Name)", new { Name = name });

			connection.ChangeDatabase(database);
			var projectTypeId = connection.QuerySingleOrDefault<int?>(getCommand);

			if (projectTypeId.HasValue)
				return;

			connection.Execute(insertCommand);
		}

		private static void AddUser(SqlConnection connection, string database)
		{
			var getCommand = new CommandDefinition("SELECT Id FROM Member WHERE AuthProviderId = @AuthProviderId", new { AuthProviderId = TestUserProvider.UserId });
			var inserCommand = new CommandDefinition("INSERT INTO Member (AuthProviderId) VALUES(@AuthProviderId)", new { AuthProviderId = TestUserProvider.UserId });

			connection.ChangeDatabase(database);
			var userId = connection.QuerySingleOrDefault<int?>(getCommand);

			if (userId.HasValue)
				return;

			connection.Execute(inserCommand);
		}
	}

	internal static class SqlConnectionExtensions
	{
		public static bool DatabaseExists(this SqlConnection connection, string name)
		{
			var command = new CommandDefinition(
				"SELECT name FROM sys.databases WHERE name = @DatabaseName",
				new { DatabaseName = name });

			connection.ChangeDatabase("master");
			var result = connection.QueryFirstOrDefault<string>(command);

			return !string.IsNullOrWhiteSpace(result);
		}

		public static void DropDatabase(this SqlConnection connection, string name)
		{
			var query =
				$@"ALTER DATABASE {name}
				SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
				DROP DATABASE {name}";

			connection.ChangeDatabase("master");
			connection.Execute(new CommandDefinition(query));
		}

		public static void CreateDatabase(this SqlConnection connection, string name)
		{
			var query = $@"CREATE DATABASE {name}";

			connection.ChangeDatabase("master");
			connection.Execute(new CommandDefinition(query));
		}

		public static void CreateTable(this SqlConnection connection, string database, string name)
		{
			var path = Environment.GetEnvironmentVariable("DATABASE_PROJECT_PATH");
			path = @"D:\Source\GitHub\rat\src\Rat.Database";

			var query = File.ReadAllText(Path.Combine(path, "Tables", name));

			connection.ChangeDatabase(database);
			connection.Execute(new CommandDefinition(query));
		}
	}
}
