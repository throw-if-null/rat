namespace Rat.Api.Routes.PatchConfigurationEntry.Data
{
	public record PatchConfigurationEntryRouteOutput(
		int Id,
		string Key,
		string Value,
		int SecondsToLive,
		bool Disabled)
	{
	}
}
