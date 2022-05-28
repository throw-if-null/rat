namespace Rat.Api.Routes.Data
{
	public record GetProjectRouteOutput(
		int Id,
		string Name,
		int TypeId,
		IEnumerable<ConfigurationInfo> Configurations);

	public record ConfigurationInfo(
		int Id,
		string Name,
		int TypeId,
		int EntriesCount);
}
