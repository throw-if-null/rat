using MediatR;
using Rat.Api.Routes.Data;

namespace Rat.Api.Routes
{
	public static class CreateProjectRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(
						"/api/project",
						(CreateProjectRouteInput model, IMediator mediator) => { return new CreateProjectRouteOutput(1, "test", 1); })
					.RequireAuthorization()
					.WithName("CreateProject");

			return builder;
		}
	}
}
