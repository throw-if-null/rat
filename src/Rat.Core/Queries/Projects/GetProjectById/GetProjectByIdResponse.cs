using System.Collections.Generic;

namespace Rat.Queries.Projects.GetProjectById
{
	internal record GetProjectByIdResponse
    {
		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }

		public IEnumerable<ConfigurationInfo> Configurations { get; init; }
	}

	internal record ConfigurationInfo(
		int Id,
		string Name,
		int ConfigurationTypeId,
		int EntriesCount);
}
