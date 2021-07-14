using Rat.Data.Views;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserResponse
    {
        public RatContext Context { get; init; }

        public UserProjectStatsView UserProjectStats { get; init; }
    }
}
