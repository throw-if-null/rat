using MediatR;
using Rat.Api.Auth;
using Rat.Core.Commands.Configurations.DeleteConfiguration;

namespace Rat.Api.Routes.DeleteConfiguration
{
	public static class DeleteConfigurationRoute
	{
		private const string ROUTE_NAME = "Delete Configuration";
		private const string ROUTE_PATH = "/api/projects/{projectId:int}/configurations/{id:int}";
		private const string TAG = "Configurations";
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
				int projectId,
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
							() => mediator.Send(new DeleteConfigurationRequest { ConfigurationId = id, ProjectId = projectId, DeletedBy = memberId }),
							_ => Results.NoContent());

				return response;
			}
		}
	}
}
