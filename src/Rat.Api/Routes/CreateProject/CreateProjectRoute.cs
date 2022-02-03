using MediatR;
using Rat.Api.Auth;
using Rat.Api.Routes.Data;
using Rat.Core;
using Rat.Core.Commands.Projects.CreateProject;

namespace Rat.Api.Routes
{
	public static class CreateProjectRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPost(
						"/api/project",
						async (CreateProjectRouteInput model, IMediator mediator, IUserProvider userProvider) =>
						{
							var userId = userProvider.GetUserId();

							if (string.IsNullOrWhiteSpace(userId))
								return Results.Forbid();

							var response = await mediator.Send(new CreateProjectRequest { Name = model.Name, ProjectTypeId = model.TypeId, UserId = userId });

							if (response.Context.Status != ProcessingStatus.Ok)
								return HttpResponseHandler.HandleUnscusseful(response.Context);

							return Results.Ok(new CreateProjectRouteOutput(response.Project.Id, response.Project.Name, response.Project.TypeId));
						})
					.RequireAuthorization()
					.WithName("CreateProject");

			return builder;
		}
	}
}
