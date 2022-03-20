using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.DataAccess;
using Rat.DataAccess.Entities;
using Rat.DataAccess.Views;
using Rat.Queries.Users.GetUserByUserId;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal class GetProjectsForUserQuery : IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>
	{
		private const string SqlQuery = "SELECT Id, Name FROM UserProject WHERE UserId = @UserId";

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

			await using var connection = _connectionFactory.CreateConnection();

			var projects =
				await
					connection.QueryAsync<ProjectEntity>(
						new CommandDefinition(
							SqlQuery,
							new { UserId = id },
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