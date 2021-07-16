using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Rat.Api.Observability.Health.Responses;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test
{
    [Collection("Integration")]
    public class HealthProbeTests
    {
        private readonly HttpClient _client;

        public HealthProbeTests(RatFixture fixture)
        {
            _client = fixture.Client;
        }

        [Theory]
        [InlineData("ready", HttpStatusCode.OK)]
        [InlineData("live", HttpStatusCode.ServiceUnavailable)]
        public async Task Probe_Should_Return_Response(string path, HttpStatusCode httpStatusCode)
        {
            var healthResponse = await _client.GetAsync($"/health/{path}");
            Assert.Equal(httpStatusCode, healthResponse.StatusCode);

            using (var stream = await healthResponse.Content.ReadAsStreamAsync())
            {
                var healthCheckResponse = await JsonSerializer.DeserializeAsync<HealthCheckResponse>(stream);

                Snapshot.Match(
                    healthCheckResponse,
                    $"{nameof(HealthProbeTests)}.{path}.{nameof(Probe_Should_Return_Response)}",
                    options => options.IgnoreField<double>("HealthChecks[*].Report.ElapsedMilliseconds"));
            }
        }
    }
}
