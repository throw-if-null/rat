using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Test._.Mocks;
using Rat.DataAccess.Projects;
using Rat.DataAccess.Users;

namespace Rat.Api.Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient();

                services.AddTransient<IProjectRepository, TestProjectRepository>();
                services.AddTransient<IUserRepository, TestUserRepository>();
            });
        }
    }
}
