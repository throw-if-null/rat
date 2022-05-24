namespace Rat.Queries.Projects.GetProjectById
{
	internal record GetProjectByIdResponse
    {
		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }
		public int ConfigurationsCount { get; init; }
		public int EntriesCount { get; init; }
	}
}
