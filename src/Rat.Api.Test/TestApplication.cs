using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rat.Api.Auth;
using Rat.Api.Test.Auth;
using Rat.Api.Test.Mocks;
using Rat.Sql;

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

				services.AddSingleton<IMemberProvider, TestMemberProvider>();

				services
					.AddAuthentication("Test")
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

				using var provider = services.BuildServiceProvider();
				using var scope = provider.CreateScope();

				var scopedServices = scope.ServiceProvider;
				var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
				var logger = scopedServices.GetRequiredService<ILogger<TestApplication>>();

				//using var connection = connectionFactory.CreateConnection();
				//connection.ConnectionString = connection.ConnectionString.Replace("RatDb", "master");
				//connection.Open();

				//if (connection.DatabaseExists("RatDb"))
				//	connection.DropDatabase("RatDb");

				//connection.CreateDatabase("RatDb");
				//connection.CreateTable("RatDb", "ProjectType.sql");
				//connection.CreateTable("RatDb", "Member.sql");
				//connection.CreateTable("RatDb", "Project.sql");
				//connection.CreateTable("RatDb", "MemberProject.sql");
				//connection.CreateTable("RatDb", "ConfigurationType.sql");
				//connection.CreateTable("RatDb", "ConfigurationRoot.sql");
				//connection.CreateTable("RatDb", "ConfigurationEntry.sql");

				//var userId = AddUser(connection, "RatDb");
				//AddProjectType(connection, "RatDb", "js", userId);
				//AddProjectType(connection, "RatDb", "csharp", userId);
			});

			return base.CreateHost(builder);
		}

		private static void AddProjectType(SqlConnection connection, string database, string name, int userId)
		{
			var getCommand = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name = @Name", new { Name = name });
			var insertCommand = new CommandDefinition("INSERT INTO ProjectType (Name, CreatedBy, ModifiedBy) VALUES(@Name, @CreatedBy, @ModifiedBy)", new { Name = name, CreatedBy = userId, ModifiedBy = userId });

			connection.ChangeDatabase(database);
			var projectTypeId = connection.QuerySingleOrDefault<int?>(getCommand);

			if (projectTypeId.HasValue)
				return;

			connection.Execute(insertCommand);
		}

		private static int AddUser(SqlConnection connection, string database)
		{
			var getCommand = new CommandDefinition("SELECT Id FROM Member WHERE AuthProviderId = @AuthProviderId", new { AuthProviderId = TestMemberProvider.MemberId });
			var inserCommand = new CommandDefinition("INSERT INTO Member (AuthProviderId) VALUES(@AuthProviderId);", new { AuthProviderId = TestMemberProvider.MemberId });

			connection.ChangeDatabase(database);
			var userId = connection.QuerySingleOrDefault<int?>(getCommand);

			if (userId.HasValue)
				return userId.Value;

			connection.Execute(inserCommand);

			userId = connection.QuerySingleOrDefault<int?>(getCommand);

			return userId.Value;
		}
	}
}
