using MediatR;
using Rat.Api.Routes.Data;
using Rat.Core;
using Rat.Core.Commands.Projects.PatchProject;

namespace Rat.Api.Routes
{
	internal static class UpdateProjectRoute
	{
		public static RouteHandlerBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapPut(
						"/api/project",
						async (UpdateProjectRouteInput input, IMediator mediator) =>
						{
							var response = await mediator.Send(new PatchProjectRequest { Id = input.Id, Name = input.Name, ProjectTypeId = input.TypeId });

							if (response.Context.Status != ProcessingStatus.Ok)
								return HttpResponseHandler.HandleUnscusseful(response.Context);

							return Results.Ok(new UpdateProjectRouteOutput(response.Project.Id, response.Project.Name, response.Project.TypeId));
						})
					.RequireAuthorization()
					.WithName("UpdateProject");

			return builder;
		}
	}
}
