using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Test.Auth;

namespace Rat.Api.Test
{
    public class RatFixture
    {
        private readonly CustomWebApplicationFactory _factory = new CustomWebApplicationFactory();

        public HttpClient Client { get; }
        public IConfiguration Configuration { get; }
        public IServiceProvider Provider { get; }

        public RatFixture()
        {
            var hostBuilder = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                });

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

            Provider = _factory.Factories[0].Services;
        }
    }
}
