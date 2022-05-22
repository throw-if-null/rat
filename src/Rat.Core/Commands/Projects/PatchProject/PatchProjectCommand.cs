using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Commands.Projects.PatchProject
{
	internal class PatchProjectCommand : IRequestHandler<PatchProjectRequest, PatchProjectResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public PatchProjectCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<PatchProjectResponse> Handle(PatchProjectRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var project = await connection.ProjectGetById(request.Id, cancellationToken);
			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.Id} does not exist");

			var projectType = await connection.ProjectTypeGetById(request.ProjectTypeId, cancellationToken);
			if (projectType == null)
				throw new ResourceNotFoundException($"ProjectType: {request.ProjectTypeId} does not exist");

			await connection.ProjectUpdate(
				project.Name == request.Name ? null : request.Name,
				project.ProjectTypeId == request.ProjectTypeId ? null : request.ProjectTypeId,
				request.Id,
				request.ModifiedBy,
				cancellationToken);

			project = await connection.ProjectGetById(request.Id, cancellationToken);

			return new()
			{
				Id = project.Id,
				Name = project.Name,
				TypeId = project.ProjectTypeId,
				ModifiedBy = project.Operator,
				ModifiedOn = project.Timestamp
			};
		}
	}
}
