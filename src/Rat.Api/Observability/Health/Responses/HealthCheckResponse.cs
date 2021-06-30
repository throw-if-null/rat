using System.Collections.Generic;

namespace Rat.Api.Observability.Health.Responses
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }

        public IEnumerable<HealthCheckInfo> HealthChecks { get; set; }
    }
}
