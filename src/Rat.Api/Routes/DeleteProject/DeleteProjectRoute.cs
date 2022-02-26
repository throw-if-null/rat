using MediatR;
using Rat.Core;
using Rat.Core.Commands.Projects.DeleteProject;

namespace Rat.Api.Routes
{
	internal static class DeleteProjectRoute
	{
		private const string ROUTE_NAME = "DeleteProject";
		private const string ROUTE_PATH = "/api/projects/{id:int}";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapDelete(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Produces(StatusCodes.Status204NoContent)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async static Task<IResult> ProcessInput(int id, IMediator mediator)
			{
				var response = await mediator.Send(new DeleteProjectRequest { Id = id });

				if (response.Context.Status != ProcessingStatus.Ok)
					return HttpResponseHandler.HandleUnscusseful(response.Context);

				return Results.NoContent();
			}
		}
	}
}
