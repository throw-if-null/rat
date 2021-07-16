using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Rat.Core.Commands.Projects.CreateProject;
using Rat.Core.Commands.Projects.DeleteProject;
using Rat.Core.Commands.Projects.PatchProject;
using Rat.Core.Queries.Projects.GetProjectById;
using Rat.Core.Queries.Projects.GetProjectsForUser;

namespace Rat.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
        {
            services.AddMediatR(
                new Type[5] {
                    typeof(CreateProjectRequest),
                    typeof(DeleteProjectRequest),
                    typeof(PatchProjectRequest),
                    typeof(GetProjectsForUserRequest),
                    typeof(GetProjectByIdRequest)
                });

            services.AddTransient<IRequestHandler<CreateProjectRequest, CreateProjectResponse>, CreateProjectCommand>();
            services.AddTransient<IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>, DeleteProjectCommand>();
            services.AddTransient<IRequestHandler<PatchProjectRequest, PatchProjectResponse>, PatchProjectCommand>();
            services.AddTransient<IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>, GetProjectsForUserQuery>();
            services.AddTransient<IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>, GetProjectByIdQuery>();

            return services;
        }
    }
}
