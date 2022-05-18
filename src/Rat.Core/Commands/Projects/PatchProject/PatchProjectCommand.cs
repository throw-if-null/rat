using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Core.Queries.ProjectTypes;
using Rat.DataAccess;
using Rat.DataAccess.Entities;
using Rat.Queries.Projects.GetProjectById;
using Rat.Sql;

namespace Rat.Commands.Projects.PatchProject
{
	internal class PatchProjectCommand : IRequestHandler<PatchProjectRequest, PatchProjectResponse>
	{
		private const string SqlQuery =
			@"UPDATE Project 
			SET Name = @Name, ProjectTypeId = @ProjectTypeId, Modified = GETUTCDATE() 
            WHERE Id = @Id; 
            SELECT Id, Name, ProjectTypeId FROM Project WHERE Id = @Id";

		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public PatchProjectCommand(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
		}

		public async Task<PatchProjectResponse> Handle(PatchProjectRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();
			var project = await connection.ProjectGetById(request.Id);
			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.Id} does not exist");

			var projectType = await connection.ProjectTypeGetById(request.ProjectTypeId);
			if (projectType == null)
				throw new ResourceNotFoundException($"ProjectType: {request.ProjectTypeId} does not exist");

			var command = new CommandDefinition(
				SqlQuery,
				new { Id = request.Id, Name = request.Name, ProjectTypeId = request.ProjectTypeId });

			// TODO: replace 1 with UserId
			await connection.ProjectUpdate(
				project.Name == request.Name ? null : request.Name,
				project.ProjectTypeId == request.ProjectTypeId ? null : request.ProjectTypeId,
				request.Id,
				1);

			project = await connection.ProjectGetById(request.Id);

			return new()
			{
				Id = project.Id,
				Name = project.Name,
				TypeId = project.ProjectTypeId
			};
		}
	}
}
