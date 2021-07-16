using System;
using System.Linq;
using System.Threading;
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
        private const string LocalDbConnectionString = "Data Source=localhost;Initial Catalog=RatDb;User ID=sa;Password=Password1!;Connect Timeout=30;";

        private int _initialized;

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

                services.AddDbContext<RatDbContext>(options => options.UseSqlServer(LocalDbConnectionString));

                using var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<RatDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                try
                {
                    var projectType = db.ProjectTypes.FirstOrDefault(x => x.Name == "js");

                    if (projectType == null)
                        db.ProjectTypes.Add(new ProjectType { Name = "js" });

                    projectType = db.ProjectTypes.FirstOrDefault(x => x.Name == "csharp");
                    if (projectType == null)
                        db.ProjectTypes.Add(new ProjectType { Name = "csharp" });

                    if (projectType == null)
                        db.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Seeding database failed. Error: {Message}", ex.Message);

                    throw;
                }
            });
        }
    }
}
