using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Core.Commands.Projects.CreateProject;
using Rat.Data;
using Rat.Data.Entities;
using Rat.Data.Exceptions;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal class PatchProjectCommand : IRequestHandler<PatchProjectRequest, PatchProjectResponse>
    {
        private readonly RatDbContext _context;

        public PatchProjectCommand(RatDbContext context)
        {
            _context = context;
        }

        public async Task<PatchProjectResponse> Handle(PatchProjectRequest request, CancellationToken cancellationToken)
        {
            ValidateId(request);
            ValidateName(request);
            var projectType = await ValidateProjectType(request, cancellationToken);

            if (request.Context.Status == ProcessingStatus.BadRequest)
                return new()
                {
                    Context = request.Context
                };

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (project == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new()
                {
                    Context = request.Context
                };
            }

            project.Name = request.Name;
            project.Type = projectType;

            _context.Projects.Update(project);
            var changes = await _context.SaveChangesAsync(cancellationToken);

            if (changes != 1)
                throw new RatDbException($"Number of changes: {changes} is not 1");

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Context = request.Context,
                Project = new() { Id = project.Id, Name = project.Name, TypeId = project.Type.Id }
            };
        }

        private static void ValidateId(PatchProjectRequest request)
        {
            if (request.Id <= 0)
                request.Context.ValidationErrors.Add(
                    $"{nameof(PatchProjectRequest)}.{nameof(PatchProjectRequest.Id)}",
                    "Id must be larger then 0");

            if (request.Context.ValidationErrors.Any())
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = "Bad request";
            }
        }

        private static void ValidateName([NotNull] PatchProjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    "Cannot be null or empty");

            if (request.Name?.Length > 248)
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    $"Lenght: {request.Name.Length} cannot be longer then 248 characters");
        }

        private async Task<ProjectType> ValidateProjectType(
            [NotNull] PatchProjectRequest request,
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
