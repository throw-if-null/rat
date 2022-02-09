using MediatR;
using Rat.Core;
using Rat.Core.Commands.Projects.DeleteProject;

namespace Rat.Api.Routes
{
	internal static class DeleteProjectRoute
	{
		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapDelete(
						"/api/projects/{id:int}",
						async (int id, IMediator mediator) =>
						{
							var response = await mediator.Send(new DeleteProjectRequest { Id = id });

							if (response.Context.Status != ProcessingStatus.Ok)
								return HttpResponseHandler.HandleUnscusseful(response.Context);

							return Results.NoContent();
						})
					.RequireAuthorization()
					.WithName("DeleteProject")
					.Produces(StatusCodes.Status204NoContent)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;
		}
	}
}
