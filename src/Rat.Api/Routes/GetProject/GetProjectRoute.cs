using MediatR;
using Rat.Api.Routes.Data;

namespace Rat.Api.Routes
{
	internal static class GetProjectRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(
						"/api/project/{id:int}",
						(int id, IMediator mediator) =>
						{
							return new GetProjectRouteOutput(id, "test", 1);
						})
					//.RequireAuthorization()
					.WithName("GetProjectsById");

			return builder;
		}
	}
}
