using System.Collections.Generic;

namespace Rat.Api.Observability.Health.Responses
{
    public record HealthCheckResponse
    {
        public string Status { get; init; }

        public IEnumerable<HealthCheckInfo> HealthChecks { get; init; }
    }
}
