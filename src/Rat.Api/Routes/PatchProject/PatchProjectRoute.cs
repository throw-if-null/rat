using System.Net.Mime;
using MediatR;
using Rat.Api.Routes.Data;
using Rat.Commands.Projects.PatchProject;
using Rat.Core;

namespace Rat.Api.Routes
{
	internal static class PatchProjectRoute
	{
		private const string ROUTE_NAME = "PatchProject";
		private const string ROUTE_PATH = "/api/projects/{id:int}";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapMethods(ROUTE_PATH, new [] { HttpMethod.Patch.Method }, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Accepts<PatchProjectRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status200OK, typeof(PatchProjectRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async Task<IResult> ProcessInput(int id, PatchProjectRouteInput input, IMediator mediator)
			{
				var response = await mediator.Send(Request(input));

				if (response.Context.Status != ProcessingStatus.Ok)
					return HttpResponseHandler.HandleUnscusseful(response.Context);

				return Results.Ok(Output(response));
			}

			static PatchProjectRequest Request(PatchProjectRouteInput input)
			{
				return new PatchProjectRequest { Id = input.Id, Name = input.Name, ProjectTypeId = input.TypeId };
			}

			static PatchProjectRouteOutput Output(PatchProjectResponse response)
			{
				return new PatchProjectRouteOutput(response.Id, response.Name, response.TypeId);
			}
		}
	}
}
