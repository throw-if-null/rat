using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rat.Api.Auth;
using Rat.Api.Test._.Mocks;
using Rat.Data;
using Rat.Data.Entities;

namespace Rat.Api.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private const string DatabaseEngineEnvironmentVariable = "DATABASE_ENGINE";
        private const string DefaultDatabaseEngine = "sqllite";

        private const string LocalDbConnectionString = "Data Source=sqlserver;Initial Catalog=RatDb;User ID=sa;Password=Password1!;Connect Timeout=30;";

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

                var databaseEngine = Environment.GetEnvironmentVariable(DatabaseEngineEnvironmentVariable);

                if (string.IsNullOrWhiteSpace(databaseEngine))
                    databaseEngine = DefaultDatabaseEngine;

                services.AddDbContext<RatDbContext>(options =>
                {
                    if (databaseEngine.Equals(DefaultDatabaseEngine, StringComparison.InvariantCultureIgnoreCase))
                        options.UseSqlite("Data Source=RatDb.db");
                    else
                        options.UseSqlServer(LocalDbConnectionString);
                });

                using var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<RatDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();


                context.Database.EnsureDeleted();

                if (databaseEngine.Equals(DefaultDatabaseEngine, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Database.EnsureCreated();

                    try
                    {
                        var projectType = context.ProjectTypes.FirstOrDefault(x => x.Name == "js");

                        if (projectType == null)
                            context.ProjectTypes.Add(new ProjectType { Name = "js" });

                        projectType = context.ProjectTypes.FirstOrDefault(x => x.Name == "csharp");
                        if (projectType == null)
                            context.ProjectTypes.Add(new ProjectType { Name = "csharp" });

                        if (projectType == null)
                            context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Seeding database failed. Error: {Message}", ex.Message);

                        throw;
                    }
                }
                else
                    context.Database.Migrate();
            });
        }
    }
}
