namespace Rat.Core.Queries.Configurations.GetConfigurationsForProject
{
	internal record GetConfigurationsForProjectResponse
	{
		public int Id { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; set; }

		public int Entries { get; init; }
	}
}
