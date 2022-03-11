using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Rat.Commands.Projects.CreateProject;
using Rat.Commands.Projects.DeleteProject;
using Rat.Commands.Projects.PatchProject;

namespace Rat.Commands
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCommands(this IServiceCollection services)
		{
			services.AddMediatR(
				new Type[3] {
					typeof(CreateProjectRequest),
					typeof(DeleteProjectRequest),
					typeof(PatchProjectRequest)
				});

			services.AddTransient<IRequestHandler<CreateProjectRequest, CreateProjectResponse>, CreateProjectCommand>();
			services.AddTransient<IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>, DeleteProjectCommand>();
			services.AddTransient<IRequestHandler<PatchProjectRequest, PatchProjectResponse>, PatchProjectCommand>();

			return services;
		}
	}
}
