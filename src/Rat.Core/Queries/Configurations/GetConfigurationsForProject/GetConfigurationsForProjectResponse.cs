using System;

namespace Rat.Core.Queries.Configurations.GetConfigurationsForProject
{
	internal record GetConfigurationsForProjectResponse
	{
		public GetConfigurationForProjectResponse[] Configurations { get; init; } = Array.Empty<GetConfigurationForProjectResponse>();
	}

	internal record GetConfigurationForProjectResponse
	{
		public int Id { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }

		public int Entries { get; init; }
	}
}
