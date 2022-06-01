using System.Net.Mime;
using MediatR;
using Rat.Api.Routes.Data;
using Rat.Queries.Projects.GetProjectById;

namespace Rat.Api.Routes
{
	internal static class GetProjectRoute
	{
		private const string ROUTE_NAME = "Get Project by Id";
		private const string ROUTE_PATH = "/api/projects/{id:int}";
		private const string TAG = "Projects";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Produces(StatusCodes.Status200OK, typeof(GetProjectRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async static Task<IResult> ProcessInput(
				HttpContext context,
				int id,
				IMediator mediator,
				RouteExecutor executor)
			{
				var response =
					await
						executor.Execute(
							ROUTE_NAME,
							() => mediator.Send(new GetProjectByIdRequest { Id = id }),
							x => Results.Ok(new GetProjectRouteOutput(
								x.Id,
								x.Name,
								x.TypeId,
								x.Configurations.Select(c => new Data.ConfigurationInfo(c.Id, c.Name, c.ConfigurationTypeId, c.EntriesCount)))));

				return response;
			}
		}
	}
}
