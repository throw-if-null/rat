namespace Rat.Api.Routes.PatchConfiguration.Data
{
	public record PatchConfigurationRouteOutput(
		int Id,
		string Name,
		int TypeId,
		int ModifiedBy,
		DateTimeOffset modifiedOn)
	{
	}
}
