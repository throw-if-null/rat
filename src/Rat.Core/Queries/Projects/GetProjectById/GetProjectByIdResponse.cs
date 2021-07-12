using Rat.Data.Entities;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdResponse
    {
        public RatContext Context { get; init; }

        public Project Project { get; init; }
    }
}
