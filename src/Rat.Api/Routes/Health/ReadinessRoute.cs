using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Rat.Api.Observability.Health;

namespace Rat.Api.Routes.Health
{
	public static class ReadinessRoute
	{
		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
				{
					AllowCachingResponses = false,
					Predicate = (check) => check.Tags.Contains("ready"),
					ResponseWriter = HealthReportWriter.WriteResponse
				});

			return builder;
		}
	}
}
