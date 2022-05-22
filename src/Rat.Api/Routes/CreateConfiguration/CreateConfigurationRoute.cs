using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.CreateConfiguration.Data;
using Rat.Api.Routes.Data;
using Rat.Commands.Projects.CreateProject;
using Rat.Core.Commands.Configurations.CreateConfiguration;
using System.Net.Mime;

namespace Rat.Api.Routes.CreateConfiguration
{
	public static class CreateConfigurationRoute
	{
		private const string ROUTE_NAME = "CreateConfiguration";
		private const string ROUTE_PATH = @"/api/configurations";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Accepts<CreateConfigurationRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status201Created, typeof(CreateConfigurationRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden);

			return builder;

			static async Task<IResult> ProcessInput(
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
						() => mediator.Send(Request(input, memberId), context.RequestAborted),
						x => Results.CreatedAtRoute(ROUTE_NAME, null, CreateOutput(x)));

				return response;
			}

			static CreateConfigurationRequest Request(CreateConfigurationRouteInput input, int memberId)
			{
				return new CreateConfigurationRequest { Name = input.Name, ConfigurationTypeId = input.TypeId, CreatedBy = memberId };
			}

			static CreateConfigurationRouteOutput CreateOutput(CreateConfigurationResponse response)
			{
				return new CreateConfigurationRouteOutput(response.Id, response.Name, response.ConfigurationTypeId);
			}
		}
	}
}
