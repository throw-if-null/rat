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
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient();

                services.AddSingleton<IUserProvider, TestUserProvider>();

                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RatDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<RatDbContext>(options => options.UseInMemoryDatabase("RatDb"));

                var provider = services.BuildServiceProvider();

                using (var scope = provider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RatDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        db.ProjectTypes.Add(new ProjectType { Id = 1, Name = "js" });
                        db.ProjectTypes.Add(new ProjectType { Id = 2, Name = "csharp" });

                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Seeding database failed. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
