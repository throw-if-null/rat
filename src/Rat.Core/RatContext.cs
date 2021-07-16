using System.Collections.Generic;

namespace Rat.Core
{
    public sealed class RatContext
    {
        public ProcessingStatus Status { get; set; }

        public IDictionary<string, string> ValidationErrors { get; init; } = new Dictionary<string, string>();

        public string FailureReason { get; set; }
    }
}
