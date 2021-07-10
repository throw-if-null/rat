using System.Collections.Generic;
using System.Linq;

namespace Rat.Api.Observability.Health.Responses
{
    public record HealthCheckData
    {
        public string Status { get; init; }

        public string Description { get; init; }

        public double ElapsedMilliseconds { get; init; }

        public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();

        public IReadOnlyDictionary<string, object> Data { get; init; } = new Dictionary<string, object>(0);
    }
}
