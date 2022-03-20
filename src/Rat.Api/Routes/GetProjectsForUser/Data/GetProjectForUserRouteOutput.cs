namespace Rat.Api.Routes.Data
{
	public record GetProjectForUserRouteOutput(string UserId, IEnumerable<ProjectStats> ProjectStats);
}
