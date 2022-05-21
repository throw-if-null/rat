using System.Net.Mime;
using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.Commands.Projects.CreateProject;
using Rat.Core;

namespace Rat.Api.Routes
{
	public static class CreateProjectRoute
	{
		private const string ROUTE_NAME = "CreateProject";
		private const string ROUTE_PATH = @"/api/projects";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(ROUTE_PATH, ProcessInput)
					.RequireAuthorization()
					.WithName(ROUTE_NAME)
					.Accepts<CreateProjectRouteInput>(MediaTypeNames.Application.Json)
					.Produces(StatusCodes.Status201Created, typeof(CreateProjectRouteOutput), MediaTypeNames.Application.Json)
					.ProducesValidationProblem()
					.ProducesProblem(StatusCodes.Status403Forbidden);

			return builder;

			static async Task<IResult> ProcessInput(
				CreateProjectRouteInput input,
				RouteExecutor executor,
				IMediator mediator,
				IMemberProvider memberProvider)
			{
				var memberId = await memberProvider.GetMemberId(CancellationToken.None);

				if (memberId == default)
					return Results.Forbid();

				var response =
					await executor.Execute(
						ROUTE_NAME,
						() => mediator.Send(Request(input, memberId), CancellationToken.None),
						x => Results.CreatedAtRoute(ROUTE_NAME, null, CreateOutput(x)));

				return response;
			}

			static CreateProjectRequest Request(CreateProjectRouteInput input, int memberId)
			{
				return new CreateProjectRequest { Name = input.Name, ProjectTypeId = input.TypeId, CreatedBy = memberId };
			}

			static CreateProjectRouteOutput CreateOutput(CreateProjectResponse response)
			{
				return new CreateProjectRouteOutput(response.Id, response.Name, response.TypeId);
			}
		}
	}
}
