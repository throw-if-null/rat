using Rat.Data.Entities;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectResponse
    {
        public RatContext Context { get; init; }

        public Project Project { get; init; }
    }
}
