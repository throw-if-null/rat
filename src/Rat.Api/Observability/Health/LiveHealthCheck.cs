using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Rat.Api.Observability.Health
{
    /// <summary>
    /// Basic health check implementation.
    /// </summary>
    public class LiveHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Executes the health check logic
        /// </summary>
        /// <param name="context">Instance of <see cref="HealthCheckContext"/>.</param>
        /// <param name="cancellationToken">Instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="HealthCheckResult"/>.</returns>
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Dead"));
        }
    }
}