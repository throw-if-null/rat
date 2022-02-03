using MediatR;
using Rat.Core;
using Rat.Core.Commands.Projects.DeleteProject;

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
						async (int id, IMediator mediator) =>
						{
							var response = await mediator.Send(new DeleteProjectRequest { Id = id });

							if (response.Context.Status != ProcessingStatus.Ok)
								return HttpResponseHandler.HandleUnscusseful(response.Context);

							return Results.Ok();
						})
					.RequireAuthorization()
					.WithName("DeleteProject");

			return builder;
		}
	}
}
