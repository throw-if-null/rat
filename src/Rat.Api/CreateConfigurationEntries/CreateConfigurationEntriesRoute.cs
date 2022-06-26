using MediatR;
using Rat.Api.Auth;
using Rat.Api.CreateConfigurationEntries.Data;
using Rat.Api.Routes.CreateConfigurationEntry.Data;
using Rat.Core.Commands.ConfigurationEntries.CreateConfigurationEntry;
using System.Net.Mime;

namespace Rat.Api.CreateConfigurationEntries
{
	public static class CreateConfigurationEntriesRoute
	{
		private const string ROUTE_NAME = "Create Configuration Entries";
		private const string ROUTE_PATH = @"/api/configurations/{configurationId:int}/entries/batch";
		private const string TAG = "Entries";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Accepts<CreateConfigurationEntriesRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status201Created, typeof(CreateConfigurationEntriesRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden);

			return builder;

			static async Task<IResult> ProcessInput(
				int configurationId,
				HttpContext context,
				CreateConfigurationEntriesRouteInput inputs,
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
						async () =>
						{
							var ids = new List<int>();

							foreach(var input in inputs.Entries)
							{
								var response = await mediator.Send(Request(input, configurationId, memberId), context.RequestAborted);
								ids.Add(response.Id);
							}

							return ids;
						},
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

			static CreateConfigurationEntriesRouteOutput CreateOutput(IEnumerable<int> ids)
			{
				return new CreateConfigurationEntriesRouteOutput(ids);
			}
		}
	}
}
