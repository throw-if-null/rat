namespace Rat.Api.Routes.PatchConfigurationEntry.Data
{
	public record PatchConfigurationEntryRouteInput(
		int id,
		int ConfigurationId,
		string Key,
		string Value,
		int SecondsToLive,
		bool Disabled)
	{
	}
}
