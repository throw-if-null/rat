using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Rat.Api.Observability.Health;

namespace Rat.Api.Routes.Health
{
	public static class LivenessRoute
	{
		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapHealthChecks("/health/live", new HealthCheckOptions()
					{
						AllowCachingResponses = false,
						Predicate = (check) => check.Tags.Contains("live"),
						ResponseWriter = HealthReportWriter.WriteResponse
					})
					.AllowAnonymous();

			return builder;
		}
	}
}
