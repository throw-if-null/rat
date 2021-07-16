using Rat.Data.Views;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdResponse
    {
        public RatContext Context { get; init; }

        public ProjectView Project { get; init; }
    }
}
