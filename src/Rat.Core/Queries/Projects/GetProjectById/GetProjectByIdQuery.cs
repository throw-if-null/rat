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
            request.Validate();

            if (request.Context.Status != ProcessingStatus.GoodRequest)
                return new() { Context = request.Context };

            var projectId = request.Id;

            var project =
                await
                    _context.Projects
                        .Include(x => x.Type)
                        .FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);

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
