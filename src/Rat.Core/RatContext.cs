using System.Collections.Generic;
using System.Linq;

namespace Rat.Core
{
    internal sealed class RatContext
    {
        public IDictionary<string, string> ValidationErrors { get; init; } = (IDictionary<string, string>)Enumerable.Empty<KeyValuePair<string, string>>();

        public ProcessingStatus Status { get; set; }

        public string FailureReason { get; set; }
    }
}
