using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.DataAccess;
using Rat.DataAccess.Projects;

namespace Rat.Core.Commands.Projects.DeleteProject
{
    internal class DeleteProjectCommand : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
    {
        private readonly IProjectRepository _repository;

        public DeleteProjectCommand(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteProjectResponse> Handle([NotNull] DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                request.Context.ValidationErrors.Add(
                    $"{nameof(DeleteProjectRequest)}.{nameof(DeleteProjectRequest.Id)}",
                    "Id must be number larger then 0");

                request.Context.Status = ProcessingStatus.BadRequest;

                return new()
                {
                    Context = request.Context
                };
            }

            var project = _repository.Retrieve(request.Id, cancellationToken);

            if (project == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new()
                {
                    Context = request.Context
                };
            }

            await _repository.Delete(request.Id, cancellationToken);

            request.Context.Status = ProcessingStatus.Ok;

            return new DeleteProjectResponse
            {
                Context = request.Context
            };
        }
    }
}
