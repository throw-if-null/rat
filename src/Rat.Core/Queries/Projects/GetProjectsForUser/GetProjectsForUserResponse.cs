using System.Collections.Generic;
using System.Linq;
using Rat.DataAccess.Views;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal record GetProjectsForUserResponse
    {
		public string UserId { get; init; }

		public IEnumerable<ProjectStatsView> ProjectStats { get; init; } = Enumerable.Empty<ProjectStatsView>();
	}
}
