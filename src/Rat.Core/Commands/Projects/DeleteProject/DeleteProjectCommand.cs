using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Data;
using Rat.Data.Exceptions;

namespace Rat.Core.Commands.Projects.DeleteProject
{
    internal class DeleteProjectCommand : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
    {
        private readonly RatDbContext _context;

        public DeleteProjectCommand(RatDbContext context)
        {
            _context = context;
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

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (project == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new()
                {
                    Context = request.Context
                };
            }

            _context.Projects.Remove(project);

            var changes = await _context.SaveChangesAsync(cancellationToken);

            if (changes != 1)
                throw new RatDbException($"Number of changes: {changes} is not 1");

            request.Context.Status = ProcessingStatus.Ok;

            return new DeleteProjectResponse
            {
                Context = request.Context
            };
        }
    }
}
