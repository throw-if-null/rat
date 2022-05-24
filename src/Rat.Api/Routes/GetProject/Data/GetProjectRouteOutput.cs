namespace Rat.Api.Routes.Data
{
	public record GetProjectRouteOutput(int Id, string Name, int TypeId, int ConfigurationsCount, int EntriesCount);
}
