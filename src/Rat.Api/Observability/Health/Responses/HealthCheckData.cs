using System.Collections.Generic;
using System.Linq;

namespace Rat.Api.Observability.Health.Responses
{
    public class HealthCheckData
    {
        public string Status { get; set; }

        public string Description { get; set; }

        public double ElapsedMilliseconds { get; set; }

        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        public IReadOnlyDictionary<string, object> Data { get; set; } = new Dictionary<string, object>(0);
    }
}
