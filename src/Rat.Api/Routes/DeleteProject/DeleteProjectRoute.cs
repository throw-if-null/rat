using MediatR;

namespace Rat.Api.Routes
{
	internal static class DeleteProjectRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapDelete(
						"/api/project/{id:int}",
						(int id, IMediator mediator) =>
						{
							return;
						})
					.RequireAuthorization()
					.WithName("DeleteProject");

			return builder;
		}
	}
}
