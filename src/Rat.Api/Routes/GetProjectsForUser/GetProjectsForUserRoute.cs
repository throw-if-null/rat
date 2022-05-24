using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.Core.Queries.Projects;
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

			async static Task<IResult> ProcessInput(
				HttpContext context,
				IMediator mediator,
				IMemberProvider userProvider,
				RouteExecutor executor)
			{
				var memberId = await userProvider.GetMemberId(context.RequestAborted);

				if (memberId == default)
					return Results.Forbid();

				var response =
					await
						executor.Execute(
							ROUTE_NAME,
							() => mediator.Send(new GetProjectsForUserRequest { MemberId = memberId }),
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
