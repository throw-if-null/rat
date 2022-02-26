using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Core.Properties;
using Rat.Data;
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
			var projectType = await _context.ProjectTypes.FirstOrDefaultAsync(x => x.Id == request.ProjectTypeId, cancellationToken);

			request.Validate(projectType);

			if (request.Context.Status != ProcessingStatus.GoodRequest)
				return new() { Context = request.Context };

			var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (project == null)
			{
				request.Context.Status = ProcessingStatus.NotFound;

				return new() { Context = request.Context };
			}

			project.Name = request.Name;
			project.Type = projectType;

			_context.Projects.Update(project);

			var expectedNumberOfChanges = 1;
			var changes = await _context.SaveChangesAsync(cancellationToken);

			if (changes != expectedNumberOfChanges)
				throw new RatDbException(string.Format(Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch, changes, expectedNumberOfChanges));

			request.Context.Status = ProcessingStatus.Ok;

			return new()
			{
				Context = request.Context,
				Id = project.Id,
				Name = project.Name,
				TypeId = project.Type.Id
			};
		}
	}
}
