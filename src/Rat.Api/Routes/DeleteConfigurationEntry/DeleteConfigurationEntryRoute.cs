﻿using MediatR;
using Rat.Api.Auth;
using Rat.Core.Commands.ConfigurationEntries.DeleteConfigurationEntry;

namespace Rat.Api.Routes.DeleteConfigurationEntry
{
	public static class DeleteConfigurationEntryRoute
	{
		private const string ROUTE_NAME = "Delete Configuration Entry";
		private const string ROUTE_PATH = "/api/configurations/{configurationId:int}/entries/{id:int}";
		private const string TAG = "Entries";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapDelete(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.WithTags(TAG)
					.Produces(StatusCodes.Status204NoContent)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;


			async static Task<IResult> ProcessInput(
				HttpContext context,
				int configurationId,
				int id,
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
							() => mediator.Send(new DeleteConfigurationEntryRequest { Id = id, DeletedBy = memberId }),
							_ => Results.NoContent());

				return response;
			}
		}
	}
}
