using System.Collections.Generic;

namespace Rat.Core.Queries.Configurations.GetConfiguration
{
	internal class GetConfigurationResponse
	{
		public IEnumerable<ConfigurationEntry> ConfigurationEntries { get; init; }
	}

	internal record ConfigurationEntry
	{
		public int Id { get; init; }

		public string Key { get; init; }

		public string Value { get; init; }

		public bool Disabled { get; init; }

		public long Expiry { get; init; }
	}
}
