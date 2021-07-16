using System.Collections.Generic;
using System.Linq;

namespace Rat.Data.Views
{
    public record UserProjectStatsView
    {
        public int UserId { get; init; }

        public IEnumerable<ProjectStatsView> ProjectStats { get; init; } = Enumerable.Empty<ProjectStatsView>();
    }
}
