using Rat.Core;

namespace Rat.Queries.Projects.GetProjectById
{
	internal record GetProjectByIdResponse
    {
		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }
	}
}
