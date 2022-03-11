using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Rat.Queries.Projects.GetProjectById;
using Rat.Queries.Projects.GetProjectsForUser;

namespace Rat.Queries
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddQueries(this IServiceCollection services)
		{
			services.AddMediatR(
				new Type[2] {
					typeof(GetProjectsForUserRequest),
					typeof(GetProjectByIdRequest)
				});

			services.AddTransient<IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>, GetProjectsForUserQuery>();
			services.AddTransient<IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>, GetProjectByIdQuery>();

			return services;
		}
	}
}
