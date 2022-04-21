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
		private const string SqlQuery =
			@"SELECT mp.MemberId, mp.ProjectId, p.Name FROM MemberProject mp 
INNER JOIN Project p ON mp.ProjectId = p.Id 
WHERE mp.MemberId = @MemberId AND mp.Deleted IS NULL";

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

			var getUserByUserIdResponse = await _mediator.Send(new GetUserByUserIdRequest { AuthProviderId = request.MemberId });

			await using var connection = _connectionFactory.CreateConnection();

			var projects =
				await
					connection.QueryAsync<ProjectEntity>(
						new CommandDefinition(
							SqlQuery,
							new { MemberId = getUserByUserIdResponse.Id },
							cancellationToken: cancellationToken));

			return new()
			{
				UserId = getUserByUserIdResponse.Id,
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