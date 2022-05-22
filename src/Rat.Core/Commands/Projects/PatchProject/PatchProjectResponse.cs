using System;

namespace Rat.Commands.Projects.PatchProject
{
	internal record PatchProjectResponse
    {
		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }

		public int ModifiedBy { get; init; }

		public DateTimeOffset ModifiedOn { get; init; }
	}
}
