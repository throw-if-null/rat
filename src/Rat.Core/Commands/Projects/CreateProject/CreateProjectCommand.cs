using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.DataAccess;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal class CreateProjectCommand : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
    {
        private readonly IProjectRepository _repository;

        public CreateProjectCommand(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            Validate(request);

            if (request.Context.Status == ProcessingStatus.Ok)
                return new ()
                {
                    Context = request.Context
                };

            var project = await _repository.Create(new() { Name = request.Name }, cancellationToken);

            return new() { Project = project, Context = request.Context };
        }

        private void Validate([NotNull] CreateProjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    "Cannot be null or empty");

            if (request.Name.Length > 512)
                request.Context.ValidationErrors.Add(
                    $"{nameof(CreateProjectRequest)}.{nameof(CreateProjectRequest.Name)}",
                    $"Lenght: {request.Name.Length} cannot be longer then 512 characters");

            if(request.Context.ValidationErrors.Any())
            {
                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = "Bad request";
            }
        }
    }
}
