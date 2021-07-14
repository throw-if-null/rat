using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Data;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal class GetProjectByIdQuery : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
    {
        private readonly RatDbContext _context;

        public GetProjectByIdQuery(RatDbContext context)
        {
            _context = context;
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

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (project == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new() { Context = request.Context };
            }

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Context = request.Context,
                Project = new() { Id = project.Id, Name = project.Name, TypeId = project.Type.Id }
            };
        }
    }
}
