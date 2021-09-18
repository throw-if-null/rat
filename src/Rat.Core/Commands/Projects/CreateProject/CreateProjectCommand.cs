using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Core.Properties;
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
			var projectTypeId = request.ProjectTypeId;
			var userId = request.UserId;

            var projectType = await _context.ProjectTypes.FirstOrDefaultAsync(x => x.Id == projectTypeId, cancellationToken);
			var user = await _context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync(cancellationToken);

			request.Validate(projectType, user);

            if (request.Context.Status != ProcessingStatus.GoodRequest)
              return new() { Context = request.Context };

            var project =
                await
                    _context.Projects.AddAsync(
                        new ProjectEntity { Name = request.Name, Type = projectType },
                        cancellationToken);

			await _context.ProjectUsers.AddAsync(
				new ProjectUserEntity { Project = project.Entity, User = user },
				cancellationToken);

            var expectedNumberOfChanges = 2;
            var changes = await _context.SaveChangesAsync(cancellationToken);

            if (changes != expectedNumberOfChanges)
                throw new RatDbException(
					string.Format(
						Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch,
						changes,
						expectedNumberOfChanges));

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Project = new() { Id = project.Entity.Id, Name = project.Entity.Name, TypeId = project.Entity.Type.Id },
                Context = request.Context
            };
        }
    }
}
