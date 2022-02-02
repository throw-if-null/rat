using MediatR;
using Rat.Api.Routes.Data;

namespace Rat.Api.Routes
{
	internal static class UpdateProjectRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPut(
						"/api/project",
						(UpdateProjectRouteInput input, IMediator mediator) =>
						{
							return new UpdateProjectRouteOutput(1, "test", 1);
						})
					.RequireAuthorization()
					.WithName("UpdateProject");

			return builder;
		}
	}
}
