using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Commands.Properties;
using Rat.Data;
using Rat.Data.Exceptions;

namespace Rat.Commands.Projects.DeleteProject
{
	internal class DeleteProjectCommand : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
    {
        private readonly RatDbContext _context;

        public DeleteProjectCommand(RatDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteProjectResponse> Handle(
            [NotNull] DeleteProjectRequest request,
            CancellationToken cancellationToken)
        {
			request.Validate();

            var projectId = request.Id;
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);

            if (project == null)
				throw new ResourceNotFoundException($"Project: {projectId} does not exist");

            _context.Projects.Remove(project);

            var expectedNumberOfChanges = 1;
            var changes = await _context.SaveChangesAsync(cancellationToken);

            if (changes != expectedNumberOfChanges)
                throw new RatDbException(string.Format(Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch, changes, expectedNumberOfChanges));

            return new();
        }
    }
}
