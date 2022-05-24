namespace Rat.Api.Routes.CreateConfigurationEntry.Data
{
	public record CreateConfigurationEntryRouteInput(
		string Key,
		string Value,
		int SecondsToLive,
		bool Disabled)
	{
	}
}
