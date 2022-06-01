using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.CreateConfiguration.Data;
using Rat.Core.Commands.Configurations.CreateConfiguration;

namespace Rat.Api.Routes.CreateConfiguration
{
	public static class CreateConfigurationRoute
	{
		private const string ROUTE_NAME = "Create Configuration";
		private const string ROUTE_PATH = @"/api/projects/{projectId:int}/configurations";
		private const string TAG = "Configurations";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Accepts<CreateConfigurationRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status201Created, typeof(CreateConfigurationRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden);

			return builder;

			static async Task<IResult> ProcessInput(
				int projectId,
				HttpContext context,
				CreateConfigurationRouteInput input,
				RouteExecutor executor,
				IMediator mediator,
				IMemberProvider memberProvider)
			{
				var memberId = await memberProvider.GetMemberId(context.RequestAborted);

				if (memberId == default)
					return Results.Forbid();

				var response =
					await executor.Execute(
						ROUTE_NAME,
						() => mediator.Send(Request(input, projectId, memberId), context.RequestAborted),
						x => Results.CreatedAtRoute(ROUTE_NAME, null, CreateOutput(x)));

				return response;
			}

			static CreateConfigurationRequest Request(CreateConfigurationRouteInput input, int projectId, int memberId)
			{
				return new CreateConfigurationRequest
				{
					ProjectId = projectId,
					Name = input.Name,
					ConfigurationTypeId = input.TypeId,
					CreatedBy = memberId };
			}

			static CreateConfigurationRouteOutput CreateOutput(CreateConfigurationResponse response)
			{
				return new CreateConfigurationRouteOutput(response.Id, response.Name, response.ConfigurationTypeId);
			}
		}
	}
}
