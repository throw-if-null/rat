using Rat.Core;

namespace Rat.Commands.Projects.PatchProject
{
	internal record PatchProjectResponse
    {
        public RatContext Context { get; init; }

		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }
	}
}
