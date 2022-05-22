using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.Api.Routes.PatchConfiguration.Data;
using Rat.Commands.Projects.PatchProject;
using System.Net.Mime;

namespace Rat.Api.Routes.PatchConfiguration
{
	public static class PatchConfigurationRoute
	{
		private const string ROUTE_NAME = "PatchConfiguration";
		private const string ROUTE_PATH = "/api/configurations/{id:int}";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapMethods(ROUTE_PATH, new[] { HttpMethod.Patch.Method }, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Accepts<PatchConfigurationRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status200OK, typeof(PatchConfigurationRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async Task<IResult> ProcessInput(
				HttpContext context,
				int id,
				PatchConfigurationRouteInput input,
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

			static PatchProjectRequest Request(PatchConfigurationRouteInput input, int memberId)
			{
				return new PatchProjectRequest { Id = input.Id, Name = input.Name, ProjectTypeId = input.ConfigurationTypeId, ModifiedBy = memberId };
			}

			static PatchConfigurationRouteOutput Output(PatchProjectResponse response)
			{
				return new PatchConfigurationRouteOutput(
					response.Id,
					response.Name,
					response.TypeId,
					response.ModifiedBy,
					response.ModifiedOn);
			}
		}
	}
}
