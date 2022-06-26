namespace Rat.Api.Routes.GetConfiguration.Data
{
	public record GetConfigurationRouteOutput(
		int Id,
		string Name,
		int ConfigurationTypeId,
		IEnumerable<ConfigurationEntry> Entries)
	{
	}

	public record ConfigurationEntry(
			int Id,
			string Key,
			string Value,
			int SecondsToLive,
			bool Disabled)
	{
	}
}
