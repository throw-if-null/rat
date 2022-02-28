using System.Net.Mime;
using MediatR;
using Rat.Api.Routes.Data;
using Rat.Core;
using Rat.Queries.Projects.GetProjectById;

namespace Rat.Api.Routes
{
	internal static class GetProjectRoute
	{
		private const string ROUTE_NAME = "GetProjectById";
		private const string ROUTE_PATH = "/api/projects/{id:int}";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Produces(StatusCodes.Status200OK, typeof(GetProjectRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async static Task<IResult> ProcessInput(int id, IMediator mediator)
			{
				var response = await mediator.Send(new GetProjectByIdRequest { Id = id });

				if (response.Context.Status != ProcessingStatus.Ok)
					return HttpResponseHandler.HandleUnscusseful(response.Context);

				return Results.Ok(new GetProjectRouteOutput(response.Id, response.Name, response.TypeId));
			}
		}
	}
}
