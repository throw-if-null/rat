using MediatR;
using Rat.Api.Auth;
using Rat.Commands.Projects.DeleteProject;

namespace Rat.Api.Routes.Empty
{
	public static class EmptyRoute
	{
		private const string ROUTE_NAME = "Empty";
		private const string ROUTE_PATH = "/";

		public static IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoints)
		{
			var builder =
				endpoints
					.MapGet(ROUTE_PATH, ProcessInput)
					.WithName(ROUTE_NAME)
					.Produces(StatusCodes.Status200OK);

			return builder;


			static Task<IResult> ProcessInput()
			{
				return Task.FromResult<IResult>(Results.Ok("OK"));
			}
		}
	}
}
