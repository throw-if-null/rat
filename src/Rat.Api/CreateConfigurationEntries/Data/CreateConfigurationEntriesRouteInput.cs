using Rat.Api.Routes.CreateConfigurationEntry.Data;

namespace Rat.Api.CreateConfigurationEntries.Data
{
	public record CreateConfigurationEntriesRouteInput(IEnumerable<CreateConfigurationEntryRouteInput> Entries)
	{
	}
}
