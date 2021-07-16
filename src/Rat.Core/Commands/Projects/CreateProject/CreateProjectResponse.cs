using Rat.Data.Views;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectResponse
    {
        public RatContext Context { get; init; }

        public ProjectView Project { get; init; }
    }
}
