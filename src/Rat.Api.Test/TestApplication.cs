using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace Rat.Api.Test
{
	internal class TestApplication : WebApplicationFactory<Program>
	{
		private const string DatabaseEngineEnvironmentVariable = "DATABASE_ENGINE";
		private const string DefaultDatabaseEngine = "sqllite";

		private const string LocalDbConnectionString = "Host=localhost;Database=RatDb;Username=sa;Password=Password1!";

		private static readonly Func<string> GetDatabaseEngine = delegate () {
			var engine = Environment.GetEnvironmentVariable(DatabaseEngineEnvironmentVariable);

			if (string.IsNullOrWhiteSpace(engine))
				engine = DefaultDatabaseEngine;

			return engine;
		};

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

			builder.ConfigureServices(async services =>
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

				using var connection = connectionFactory.CreateConnection();

				AddProjectType(connection, "js");
				AddProjectType(connection, "csharp");

				AddUser(connection);
			});

			return base.CreateHost(builder);
		}

		private static void AddProjectType(SqlConnection connection, string name)
		{
			var getCommand = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name = @Name", new { Name = name });
			var insertCommand = new CommandDefinition("INSERT INTO ProjectType (Name) VALUES(@Name)", new { Name = name });

			var projectTypeId = connection.QuerySingleOrDefault<int?>(getCommand);

			if (projectTypeId.HasValue)
				return;

			connection.Execute(insertCommand);
		}

		private static void AddUser(SqlConnection connection)
		{
			var getCommand = new CommandDefinition("SELECT Id FROM Member WHERE AuthProviderId = @AuthProviderId", new { AuthProviderId = TestUserProvider.UserId });
			var inserCommand = new CommandDefinition("INSERT INTO Member (AuthProviderId) VALUES(@AuthProviderId)", new { AuthProviderId = TestUserProvider.UserId });

			var userId = connection.QuerySingleOrDefault<int?>(getCommand);

			if (userId.HasValue)
				return;

			connection.Execute(inserCommand);
		}
	}
}
