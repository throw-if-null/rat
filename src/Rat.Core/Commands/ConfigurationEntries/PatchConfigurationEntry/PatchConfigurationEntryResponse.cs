using System;

namespace Rat.Core.Commands.ConfigurationEntries.PatchConfigurationEntry
{
	internal class PatchConfigurationEntryResponse
	{
		public int Id { get; init; }

		public string Key { get; init; }

		public string Value { get; init; }

		public int SecondsToLive { get; init; }

		public bool Disabled { get; init; }

		public int ConfigurationRootId { get; init; }

		public int ModifiedBy { get; init; }

		public DateTimeOffset ModifiedOn { get; init; }
	}
}