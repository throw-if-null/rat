namespace Rat.Api.Routes.Data
{
	public record GetProjectForUserRouteOutput(int UserId, IEnumerable<ProjectStats> ProjectStats);
}
