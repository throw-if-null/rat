using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Rat.Api.Observability.Health.Responses;

namespace Rat.Api.Observability.Health
{
	/// <summary>
	/// Contains utility methods for configuring health checks
	/// </summary>
	public static class HealthReportWriter
    {
        /// <summary>
        /// Tries to write the health check result data into HTTP response message.
        /// </summary>
        /// <param name="httpContext">Instance of <see cref="HttpContext"/>.</param>
        /// <param name="result">Instance of <see cref="HealthReport"/>.</param>
        /// <returns>A task.</returns>
        public static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            _ = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _ = result ?? throw new ArgumentNullException(nameof(result));

            httpContext.Response.ContentType = "application/json";

            var response = new HealthCheckResponse
            {
                Status = Enum.GetName(result.Status),
                HealthChecks = result.Entries.Select(pair => new HealthCheckInfo
                {
                    Name = pair.Key,
                    Report = new HealthCheckData
                    {
                        Status = Enum.GetName(pair.Value.Status),
                        Description = pair.Value.Description,
                        ElapsedMilliseconds = pair.Value.Duration.TotalMilliseconds,
                        Tags = pair.Value.Tags,
                        Data = pair.Value.Data
                    }
                })
            };

            return httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}