using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.Commands.Projects.PatchProject;

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

			async Task<IResult> ProcessInput(
				HttpContext context,
				int id,
				PatchProjectRouteInput input,
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
							() => mediator.Send(Request(input, memberId)),
							x => Results.Ok(Output(x)));

				return response;
			}

			static PatchProjectRequest Request(PatchProjectRouteInput input, int memberId)
			{
				return new PatchProjectRequest { Id = input.Id, Name = input.Name, ProjectTypeId = input.TypeId, ModifiedBy = memberId };
			}

			static PatchProjectRouteOutput Output(PatchProjectResponse response)
			{
				return new PatchProjectRouteOutput(response.Id, response.Name, response.TypeId);
			}
		}
	}
}
