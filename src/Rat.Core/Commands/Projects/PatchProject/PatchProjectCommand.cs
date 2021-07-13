using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Commands.Projects.CreateProject;
using Rat.Data.Entities;
using Rat.DataAccess;
using Rat.DataAccess.Projects;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal class PatchProjectCommand : IRequestHandler<PatchProjectRequest, PatchProjectResponse>
    {
        private readonly IProjectRepository _repository;

        public PatchProjectCommand(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<PatchProjectResponse> Handle(PatchProjectRequest request, CancellationToken cancellationToken)
        {
            Validate(request);

            if (request.Context.Status == ProcessingStatus.BadRequest)
                return new()
                {
                    Context = request.Context
                };

            var project = await _repository.Retrieve(request.Id, cancellationToken);

            if (project == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new()
                {
                    Context = request.Context
                };
            }

            project = await _repository.Update(new Project { Id = request.Id, Name = request.Name }, cancellationToken);

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Context = request.Context,
                Project = project
            };
        }

        private static void Validate(PatchProjectRequest request)
        {
            if (request.Id <= 0)
                request.Context.ValidationErrors.Add(
                    $"{nameof(PatchProjectRequest)}.{nameof(PatchProjectRequest.Id)}",
                    "Id must be larger then 0");

            if (string.IsNullOrWhiteSpace(request.Name))
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    "Cannot be null or empty");

            if (request.Name?.Length > 512)
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    $"Lenght: {request.Name.Length} cannot be longer then 512 characters");

            if (request.Context.ValidationErrors.Any())
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = "Bad request";
            }
        }
    }
}
