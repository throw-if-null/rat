using Rat.Data.Entities;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectResponse
    {
        public RatContext Context { get; init; }

        public Project Project { get; init; }
    }
}
