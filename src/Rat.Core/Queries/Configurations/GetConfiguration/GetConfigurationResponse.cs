using System;
using System.Collections.Generic;

namespace Rat.Core.Queries.Configurations.GetConfiguration
{
	internal record GetConfigurationResponse
	{
		public int ConfigurationId { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }

		public IEnumerable<ConfigurationEntry> ConfigurationEntries { get; init; }
	}

	internal record ConfigurationEntry
	{
		public int Id { get; init; }

		public string Key { get; init; }

		public string Value { get; init; }

		public bool Disabled { get; init; }

		public int SecondsToLive { get; init; }
	}
}
