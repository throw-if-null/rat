using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rat.Api.Auth;
using Rat.Api.Test.Auth;
using Rat.Api.Test.Mocks;
using Rat.Data;
using Rat.Data.Entities;

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

			builder.ConfigureServices(services =>
			{
				services.AddHttpClient();

				services.AddSingleton<IUserProvider, TestUserProvider>();

				var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RatDbContext>));
				services.Remove(descriptor);

				services
					.AddAuthentication("Test")
					.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

				services.AddDbContext<RatDbContext>(options =>
				{
					if (GetDatabaseEngine().Equals(DefaultDatabaseEngine, StringComparison.InvariantCultureIgnoreCase))
						options.UseSqlite("Data Source=RatDb.db");
					else
						options.UseNpgsql(LocalDbConnectionString);
				});

				using var provider = services.BuildServiceProvider();
				using var scope = provider.CreateScope();

				var scopedServices = scope.ServiceProvider;
				var context = scopedServices.GetRequiredService<RatDbContext>();
				var logger = scopedServices.GetRequiredService<ILogger<TestApplication>>();

				context.Database.EnsureDeleted();

				if (GetDatabaseEngine().Equals(DefaultDatabaseEngine, StringComparison.InvariantCultureIgnoreCase))
				{
					context.Database.EnsureCreated();

					AddProjectType(context, "js");
					AddProjectType(context, "csharp");
				}
				else
				{
					context.Database.Migrate();
				}

				AddUser(context);
			});

			return base.CreateHost(builder);
		}

		private static void AddProjectType(RatDbContext context, string name)
		{
			var projectType = context.ProjectTypes.FirstOrDefault(x => x.Name == name);

			if (projectType == null)
				context.ProjectTypes.Add(new ProjectTypeEntity { Name = name });

			context.SaveChanges();
		}

		private static void AddUser(RatDbContext context)
		{
			var user = context.Users.FirstOrDefault(x => x.UserId == TestUserProvider.UserId);

			if (user == null)
			{
				context.Users.Add(new UserEntity { UserId = TestUserProvider.UserId });

				context.SaveChanges();
			}
		}
	}
}
