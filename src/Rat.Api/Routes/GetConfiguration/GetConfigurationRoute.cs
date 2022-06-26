using System.Net.Mime;
using MediatR;
using Rat.Api.Routes.GetConfiguration.Data;
using Rat.Core.Queries.Configurations.GetConfiguration;

namespace Rat.Api.Routes.GetConfiguration
{
	public static class GetConfigurationRoute
	{
		private const string ROUTE_NAME = "Get Configuration by Id";
		private const string ROUTE_PATH = "/api/configuration/{id:int}";
		private const string TAG = "Configurations";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Produces(StatusCodes.Status200OK, typeof(GetConfigurationRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async static Task<IResult> ProcessInput(
				HttpContext context,
				int id,
				IMediator mediator,
				RouteExecutor executor)
			{
				var response =
					await
						executor.Execute(
							ROUTE_NAME,
							() => mediator.Send(new GetConfigurationRequest { ConfigurationId = id }),
							x => Results.Ok(new GetConfigurationRouteOutput(
								x.ConfigurationId,
								x.Name,
								x.ConfigurationTypeId,
								x.ConfigurationEntries.Select(e => new Data.ConfigurationEntry(
									e.Id,
									e.Key,
									e.Value,
									e.SecondsToLive,
									e.Disabled)
								))));

				return response;
			}
		}
	}
}
