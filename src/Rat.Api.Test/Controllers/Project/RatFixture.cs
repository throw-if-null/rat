using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Test.Auth;

namespace Rat.Api.Test.Controllers.Project
{
    public class RatFixture
    {
        private readonly CustomWebApplicationFactory _factory = new CustomWebApplicationFactory();

        public HttpClient Client { get; }
        public IConfiguration Configuration { get; }

        public RatFixture()
        {
            var hostBuilder = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services
                        .AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                });
            });

            Client = hostBuilder.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            Configuration = hostBuilder.Services.GetRequiredService<IConfiguration>();
        }
    }
}
