using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Data;
using Rat.Data.Entities;
using Rat.Data.Exceptions;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal class CreateProjectCommand : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
    {
        private readonly RatDbContext _context;

        public CreateProjectCommand(RatDbContext context)
        {
            _context = context;
        }

        public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            ValidateName(request);
            var projectType = await ValidateProjectType(request, cancellationToken);

            if (request.Context.Status == ProcessingStatus.BadRequest)
                return new() { Context = request.Context };

            var project =
                await
                    _context.Projects.AddAsync(
                        new Project { Name = request.Name, Type = projectType },
                        cancellationToken);

            var changes = await _context.SaveChangesAsync(cancellationToken);

            if (changes != 1)
                throw new RatDbException($"Number of changes: {changes} is not 1");

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Project = new() { Id = project.Entity.Id, Name = project.Entity.Name, TypeId = project.Entity.Type.Id },
                Context = request.Context
            };
        }

        private void ValidateName([NotNull] CreateProjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    "Cannot be null or empty");

            if (request.Name?.Length > 512)
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    $"Lenght: {request.Name.Length} cannot be longer then 512 characters");
        }

        private async Task<ProjectType> ValidateProjectType(
            [NotNull] CreateProjectRequest request,
            CancellationToken cancellation)
        {
            var projectType = await _context.ProjectTypes.FirstOrDefaultAsync(x => x.Id == request.ProjectTypeId, cancellation);

            if (projectType == null)
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.ProjectTypeId)}",
                    $"Value: {request.ProjectTypeId} does not exist");
            }

            if (request.Context.ValidationErrors.Any())
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = "Invalid request data.";
            }

            return projectType;
        }
    }
}
