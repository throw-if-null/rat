using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.DataAccess.Views;
using Rat.Queries.Projects.GetProjectsForUser;

namespace Rat.Api.Routes
{
	internal static class GetProjectsForUserRoute
	{
		private const string ROUTE_NAME = "GetUserProject";
		private const string ROUTE_PATH = "/api/projects";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.ProducesValidationProblem()
					.Produces(StatusCodes.Status200OK, typeof(GetProjectForUserRouteOutput), MediaTypeNames.Application.Json)
					.ProducesProblem(StatusCodes.Status403Forbidden);

			return builder;

			async static Task<IResult> ProcessInput(IMediator mediator, IUserProvider userProvider, RouteExecutor executor)
			{
				var userId = userProvider.GetUserId();

				if (string.IsNullOrWhiteSpace(userId))
					return Results.Forbid();

				var response =
					await
						executor.Execute(
							ROUTE_NAME,
							() => mediator.Send(new GetProjectsForUserRequest { UserId = userId }),
							x => Results.Ok(Output(x)));

				return response;
			}

			static GetProjectForUserRouteOutput Output(GetProjectsForUserResponse response)
			{
				return new GetProjectForUserRouteOutput(
					response.UserId,
					response.ProjectStats.Select(ProjectStats));
			}

			static ProjectStats ProjectStats(ProjectStatsView projectStats)
			{
				return new ProjectStats(
					projectStats.Id,
					projectStats.Name,
					projectStats.TotalConfigurationCount,
					projectStats.TotalEntryCount);
			}
		}
	}
}
