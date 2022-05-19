using System.Collections.Generic;
using System.Linq;
using Rat.Core.Queries.Projects;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal record GetProjectsForUserResponse
    {
		public int UserId { get; init; }

		public IEnumerable<ProjectStatsView> ProjectStats { get; init; } = Enumerable.Empty<ProjectStatsView>();
	}
}
