using System.Collections.Generic;
using System.Linq;

namespace Rat.Data.Views
{
    public record UserProjectStats
    {
        public int UserId { get; init; }

        public IEnumerable<ProjectStats> ProjectStats { get; init; } = Enumerable.Empty<ProjectStats>();
    }
}
