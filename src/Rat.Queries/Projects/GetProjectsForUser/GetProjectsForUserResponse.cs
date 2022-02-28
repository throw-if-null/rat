using System.Collections.Generic;
using System.Linq;
using Rat.Core;
using Rat.Data.Views;

namespace Rat.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserResponse
    {
        public RatContext Context { get; init; }

		public int UserId { get; init; }

		public IEnumerable<ProjectStatsView> ProjectStats { get; init; } = Enumerable.Empty<ProjectStatsView>();
	}
}
