using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.CreateConfigurationEntry.Data;
using Rat.Core.Commands.ConfigurationEntries.CreateConfigurationEntry;

namespace Rat.Api.Routes.CreateConfigurationEntry
{
	public static class CreateConfigurationEntryRoute
	{
		private const string ROUTE_NAME = "Create Configuration Entry";
		private const string ROUTE_PATH = @"/api/configurations/{configurationId:int}/entries";
		private const string TAG = "Entries";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Accepts<CreateConfigurationEntryRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status201Created, typeof(CreateConfigurationEntryRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden);

			return builder;

			static async Task<IResult> ProcessInput(
				int configurationId,
				HttpContext context,
				CreateConfigurationEntryRouteInput input,
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
						() => mediator.Send(Request(input, configurationId, memberId), context.RequestAborted),
						x => Results.CreatedAtRoute(ROUTE_NAME, null, CreateOutput(x)));

				return response;
			}

			static CreateConfigurationEntryRequest Request(CreateConfigurationEntryRouteInput input, int configurationId, int memberId)
			{
				return new CreateConfigurationEntryRequest
				{
					ConfigurationRootId = configurationId,
					Key = input.Key,
					Value = input.Value,
					SecondsToLive = input.SecondsToLive,
					Disabled = input.Disabled,
					CraetedBy = memberId
				};
			}

			static CreateConfigurationEntryRouteOutput CreateOutput(CreateConfigurationEntryResponse response)
			{
				return new CreateConfigurationEntryRouteOutput(response.Id);
			}
		}
	}
}
