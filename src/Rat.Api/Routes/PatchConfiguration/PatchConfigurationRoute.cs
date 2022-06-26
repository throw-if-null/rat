using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.PatchConfiguration.Data;
using Rat.Core.Commands.Configurations.PatchConfiguration;

namespace Rat.Api.Routes.PatchConfiguration
{
	public static class PatchConfigurationRoute
	{
		private const string ROUTE_NAME = "Patch Configuration";
		private const string ROUTE_PATH = "/api/projects/{projectId:int}/configurations/{id:int}";
		private const string TAG = "Configurations";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapMethods(ROUTE_PATH, new[] { HttpMethod.Patch.Method }, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Accepts<PatchConfigurationRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status200OK, typeof(PatchConfigurationRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async Task<IResult> ProcessInput(
				HttpContext context,
				int projectId,
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
							() => mediator.Send(Request(input, projectId, memberId)),
							x => Results.Ok(Output(x)));

				return response;
			}

			static PatchConfigurationRequest Request(PatchConfigurationRouteInput input, int projectId, int memberId)
			{
				return new PatchConfigurationRequest
				{
					Id = input.Id,
					ConfigurationTypeId = input.ConfigurationTypeId,
					Name = input.Name,
					ProjectId = projectId,
					ModifiedBy = memberId
				};
			}

			static PatchConfigurationRouteOutput Output(PatchConfigurationResponse response)
			{
				return new PatchConfigurationRouteOutput(
					response.ConfigurationId,
					response.Name,
					response.ConfigurationTypeId);
			}
		}
	}
}
