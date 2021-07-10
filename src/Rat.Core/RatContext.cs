using System.Collections.Generic;
using System.Linq;

namespace Rat.Core
{
    internal sealed class RatContext
    {
        public IDictionary<string, string> ValidationErrors { get; init; } = (IDictionary<string, string>)Enumerable.Empty<KeyValuePair<string, string>>();

        public bool Failed { get; init; }

        public string FailureReason { get; init; }
    }
}
