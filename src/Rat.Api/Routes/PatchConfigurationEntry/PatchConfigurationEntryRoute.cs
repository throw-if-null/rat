using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.Api.Routes.PatchConfigurationEntry.Data;
using Rat.Core.Commands.ConfigurationEntries.PatchConfigurationEntry;
using System.Net.Mime;

namespace Rat.Api.Routes.PatchConfigurationEntry
{
	public static class PatchConfigurationEntryRoute
	{
		private const string ROUTE_NAME = "PatchConfigurationEntry";
		private const string ROUTE_PATH = "/api/configuration{configurationId:int}/entry/{id:int}";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapMethods(ROUTE_PATH, new[] { HttpMethod.Patch.Method }, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Accepts<PatchConfigurationEntryRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status200OK, typeof(PatchConfigurationEntryRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async Task<IResult> ProcessInput(
				int configurationId,
				int id,
				HttpContext context,
				PatchConfigurationEntryRouteInput input,
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
							() => mediator.Send(Request(input, id, configurationId, memberId)),
							x => Results.Ok(Output(x)));

				return response;
			}

			static PatchConfigurationEntryRequest Request(PatchConfigurationEntryRouteInput input, int id, int configurationId, int memberId)
			{
				return new PatchConfigurationEntryRequest
				{
					Id = id,
					Key = input.Key,
					Value = input.Value,
					SecondsToLive = input.SecondsToLive,
					Disabled = input.Disabled,
					ConfigurationRootId = configurationId,
					ModifiedBy = memberId
				};
			}

			static PatchConfigurationEntryRouteOutput Output(PatchConfigurationEntryResponse response)
			{
				return new PatchConfigurationEntryRouteOutput(response.Id, response.Key, response.Value, response.SecondsToLive, response.Disabled);
			}
		}
	}
}
