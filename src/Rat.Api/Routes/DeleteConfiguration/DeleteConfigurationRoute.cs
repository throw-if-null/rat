﻿using MediatR;
using Rat.Api.Auth;
using Rat.Commands.Projects.DeleteProject;
using Rat.Core.Commands.Configurations.DeleteConfiguration;

namespace Rat.Api.Routes.DeleteConfiguration
{
	public static class DeleteConfigurationRoute
	{
		private const string ROUTE_NAME = "DeleteConfiguration";
		private const string ROUTE_PATH = "/api/configurations/{id:int}";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapDelete(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Produces(StatusCodes.Status204NoContent)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden)
					.ProducesProblem(StatusCodes.Status404NotFound);

			return builder;

			async static Task<IResult> ProcessInput(
				HttpContext context,
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
							() => mediator.Send(new DeleteConfigurationRequest { ConfigurationId = id, DeletedBy = memberId }),
							_ => Results.NoContent());

				return response;
			}
		}
	}
}
