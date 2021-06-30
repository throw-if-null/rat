using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Rat.Api.Observability.Health.Responses;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test
{
    public class HealthProbeTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public HealthProbeTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Readiness_Should_Return_Unhealthy_Response()
        {
            var healthResponse = await _client.GetAsync("/health/ready");

            using (var stream = await healthResponse.Content.ReadAsStreamAsync())
            {
                var healthCheckResponse = await JsonSerializer.DeserializeAsync<HealthCheckResponse>(stream);

                Snapshot.Match(
                    healthCheckResponse,
                    options => options.IgnoreField<double>("HealthChecks[*].Report.ElapsedMilliseconds"));
            }
        }
    }
}
