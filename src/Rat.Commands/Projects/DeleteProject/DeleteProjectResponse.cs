using Rat.Core;

namespace Rat.Commands.Projects.DeleteProject
{
	internal record DeleteProjectResponse
    {
        public RatContext Context { get; init; }
    }
}
