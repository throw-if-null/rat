using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.DataAccess;
using Rat.DataAccess.Projects;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal class GetProjectByIdQuery : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
    {
        private readonly IProjectRepository _repository;

        public GetProjectByIdQuery(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProjectByIdResponse> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                request.Context.ValidationErrors.Add(
                    $"{nameof(GetProjectByIdRequest)}.{nameof(GetProjectByIdRequest.Id)}",
                    "Id must have larger value then 0");

                request.Context.Status = ProcessingStatus.BadRequest;

                return new() { Context = request.Context };
            }

            var project = await _repository.Retrieve(request.Id, cancellationToken);

            if (project == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new() { Context = request.Context };
            }

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Context = request.Context,
                Project = project
            };
        }
    }
}
