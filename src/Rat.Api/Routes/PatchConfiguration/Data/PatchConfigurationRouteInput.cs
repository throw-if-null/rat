namespace Rat.Api.Routes.PatchConfiguration.Data
{
	public record PatchConfigurationRouteInput(
		int Id,
		string Name,
		int ConfigurationTypeId)
	{
	}
}
