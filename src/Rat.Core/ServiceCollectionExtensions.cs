using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Rat.Commands.Projects.CreateProject;
using Rat.Commands.Projects.DeleteProject;
using Rat.Commands.Projects.PatchProject;
using Rat.Commands.Users.CreateUser;
using Rat.Queries.Projects.GetProjectById;
using Rat.Queries.Projects.GetProjectsForUser;

namespace Rat.Core
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
			services.AddTransient<IRequestHandler<CreateUserRequest, CreateUserResponse>, CreateUserCommand>();

			return services;
		}

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
