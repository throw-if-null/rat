using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Data;
using Rat.Data.Exceptions;

namespace Rat.Queries.Projects.GetProjectById
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

			var projectId = request.Id;

			var project =
				await
					_context.Projects
						.Include(x => x.Type)
						.FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);

			if (project == null)
				throw new ResourceNotFoundException($"Project: {projectId} does not exist");

			return new()
			{
				Id = project.Id,
				Name = project.Name,
				TypeId = project.Type.Id
			};
		}
	}
}
