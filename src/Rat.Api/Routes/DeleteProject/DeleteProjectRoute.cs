using MediatR;
using Rat.Api.Auth;
using Rat.Commands.Projects.DeleteProject;

namespace Rat.Api.Routes
{
	internal static class DeleteProjectRoute
	{
		private const string ROUTE_NAME = "Delete Project";
		private const string ROUTE_PATH = "/api/projects/{id:int}";
		private const string TAG = "Projects";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapDelete(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Produces(StatusCodes.Status204NoContent)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;


			async static Task<IResult> ProcessInput(
				HttpContext context,
				int id,
				IMediator mediator,
				IMemberProvider memberProvider,
				RouteExecutor executor)
{
				var memberId = await memberProvider.GetMemberId(context.RequestAborted);

				if (memberId == default)
					return Results.Forbid();

				var response =
					await
						executor.Execute(
							ROUTE_NAME,
							() => mediator.Send(new DeleteProjectRequest { Id = id, DeletedBy = memberId}),
							_ => Results.NoContent());

				return response;
			}
		}
	}
}
