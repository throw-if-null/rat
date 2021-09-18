using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rat.Api.Auth;
using Rat.Api.Test.Mocks;
using Rat.Data;
using Rat.Data.Entities;

namespace Rat.Api.Test
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
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

		public CustomWebApplicationFactory() : base()
		{
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				services.AddHttpClient();

				services.AddSingleton<IUserProvider, TestUserProvider>();

				var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RatDbContext>));
				services.Remove(descriptor);

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
				var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

				context.Database.EnsureDeleted();

				if (GetDatabaseEngine().Equals(DefaultDatabaseEngine, StringComparison.InvariantCultureIgnoreCase))
				{
					context.Database.EnsureCreated();

					var projectType = context.ProjectTypes.FirstOrDefault(x => x.Name == "js");

					if (projectType == null)
						context.ProjectTypes.Add(new ProjectTypeEntity { Name = "js" });

					projectType = context.ProjectTypes.FirstOrDefault(x => x.Name == "csharp");

					if (projectType == null)
						context.ProjectTypes.Add(new ProjectTypeEntity { Name = "csharp" });

					if (projectType == null)
						context.SaveChanges();

					var user = context.Users.FirstOrDefault(x => x.UserId == "3feslrj3ssd111");

					if (user == null)
					{
						context.Users.Add(new UserEntity { UserId = "3feslrj3ssd111" });

						context.SaveChanges();
					}
				}
				else
				{
					context.Database.Migrate();
				}
			});
		}
	}
}
