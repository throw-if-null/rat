using Rat.Data.Views;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectResponse
    {
        public RatContext Context { get; init; }

        public ProjectView Project { get; init; }
    }
}
