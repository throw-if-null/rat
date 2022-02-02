using MediatR;
using Rat.Api.Routes.Data;

namespace Rat.Api.Routes
{
	internal static class GetProjectsForUserRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(
						"/api/project",
						(IMediator mediator) =>
							{
								return new GetProjectForUserRouteOutput(
									1,
									new List<ProjectStats>
									{
										new (1, "test", 1, 4),
										new (2, "test2", 2, 3)
									});
							})
					.RequireAuthorization()
					.WithName("GetProjectsForUser");

			return builder;
		}
	}
}
