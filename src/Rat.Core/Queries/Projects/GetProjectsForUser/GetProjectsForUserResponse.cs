using Rat.Data.Views;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserResponse
    {
        public RatContext Context { get; init; }

        public UserProjectStats UserProjectStats { get; init; }
    }
}
