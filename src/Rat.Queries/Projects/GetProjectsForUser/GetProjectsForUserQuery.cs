using Dapper;
using MediatR;
using Rat.Data.Views;
using Rat.DataAccess;
using Rat.DataAccess.Entities;
using Rat.Queries.Users.GetUserByUserId;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal class GetProjectsForUserQuery : IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>
	{

		private const string INSERT_USER =
			@"INSERT INTO USER (UserId) VALUES (@UserId); 
		     SELECT SCOPE_IDENTITY()";

		private const string SELECT_PROJECT_STATS = "SELECT Id, Name FROM UserProject WHERE UserId = @UserId";

		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public GetProjectsForUserQuery(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
		}

		public async Task<GetProjectsForUserResponse> Handle(GetProjectsForUserRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			var id = await _mediator.Send(new GetUserByUserIdRequest { AuthProviderUserId = request.UserId });

			//if (!id.HasValue)
			//{
			//	var command = new CommandDefinition(INSERT_USER, new { UserId = request.UserId }, cancellationToken: cancellationToken);

			//	id = await connection.QuerySingleAsync<int>(command);
			//}

			await using var connection = _connectionFactory.CreateConnection();

			var projects =
				await
					connection.QueryAsync<ProjectEntity>(
						new CommandDefinition(
							SELECT_PROJECT_STATS,
							new { UserId = request.UserId },
							cancellationToken: cancellationToken));

			return new()
			{
				UserId = request.UserId,
				ProjectStats = projects.Select(x => new ProjectStatsView
				{
					Id = x.Id,
					Name = x.Name,
					TotalConfigurationCount = 0,
					TotalEntryCount = 0
				})
			};
		}
	}
}