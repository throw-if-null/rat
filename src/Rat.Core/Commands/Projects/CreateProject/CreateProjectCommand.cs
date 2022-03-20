using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Queries.ProjectTypes;
using Rat.DataAccess;
using Rat.Queries.Users.GetUserByUserId;

namespace Rat.Commands.Projects.CreateProject
{
	internal class CreateProjectCommand : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
	{
		private const string SqlQuery =
			@"INSERT INTO Project (Name, ProjectTypeId) VALUES(@Name, @ProjectTypeId)
            DECLARE @ProjectId INT = (SELECT @@SCOPE_IDENTITY())
            INSERT INTO UserProject (UserId, ProjectId) VALUES (@UserId, @ProjectId)
			SELECT @ProjectId";

		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public CreateProjectCommand(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
		}

		public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
		{
			var getProjectTypeByIdResponse = await _mediator.Send(new GetProjectTypeByIdRequest { Id = request.ProjectTypeId });
			var getUserByUserIdResponse = await _mediator.Send(new GetUserByUserIdRequest { AuthProviderUserId = request.UserId });

			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(
				SqlQuery,
				new { Name = request.Name, ProjectTypeId = getProjectTypeByIdResponse.Id, UserId = getUserByUserIdResponse.Id },
				cancellationToken: cancellationToken);

			var projectId = await connection.QuerySingleAsync<int>(command);

			return new()
			{
				Id = projectId,
				Name = request.Name,
				TypeId = getProjectTypeByIdResponse.Id
			};
		}
	}
}
