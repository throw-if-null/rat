using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rat.Api.Test
{
	public class RatFixture
    {
        private readonly TestApplication _application = new TestApplication();

        public HttpClient Client { get; }
        public IConfiguration Configuration { get; }
        public IServiceProvider Provider { get; }

		public JsonSerializerOptions JsonSerializerOption => new()
		{
			PropertyNameCaseInsensitive = true
		};

		public RatFixture()
        {
            Client = _application.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
				BaseAddress = new Uri("http://localhost")
            });

            Configuration = _application.Services.GetRequiredService<IConfiguration>();

			Provider = _application.Services;
		}
    }
}
