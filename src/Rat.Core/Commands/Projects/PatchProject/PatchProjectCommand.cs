using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Core.Queries.ProjectTypes;
using Rat.DataAccess;
using Rat.DataAccess.Entities;
using Rat.Queries.Projects.GetProjectById;

namespace Rat.Commands.Projects.PatchProject
{
	internal class PatchProjectCommand : IRequestHandler<PatchProjectRequest, PatchProjectResponse>
	{
		private const string SqlQuery =
			@"UPDATE Project SET Name = @Name, ProjectTypeId = @ProjectTypeId, Modified = GETUTCDATE() WHERE Id = @Id; 
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

			var projectType = await _mediator.Send(new GetProjectTypeByIdRequest { Id = request.ProjectTypeId });

			if (projectType == null)
				throw new ResourceNotFoundException($"ProjectType: {request.ProjectTypeId} does not exist");

			var getProjectByIdResponse = await _mediator.Send(new GetProjectByIdRequest { Id = request.Id });

			if (getProjectByIdResponse  == null)
				throw new ResourceNotFoundException($"Project: {request.Id} does not exist");

			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(
				SqlQuery,
				new { Id = request.Id, Name = request.Name, ProjectTypeId = request.ProjectTypeId });

			var project = await connection.QuerySingleAsync<ProjectEntity>(command);

			return new()
			{
				Id = project.Id,
				Name = request.Name,
				TypeId = request.ProjectTypeId
			};
		}
	}
}
