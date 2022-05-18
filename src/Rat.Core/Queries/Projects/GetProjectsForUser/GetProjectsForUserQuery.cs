using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.DataAccess;
using Rat.DataAccess.Entities;
using Rat.DataAccess.Views;
using Rat.Queries.Users.GetUserByUserId;
using Rat.Sql;

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

			await using var connection = _connectionFactory.CreateConnection();

			var member = await connection.MemberGetByAuthProviderId(request.MemberId);

			if (member == null)
			{
				member = new { Id = 0 };
				member.Id = await connection.MemberInsert(request.MemberId, 1);
			}

			var projects = await ProjectSqlConnectionExtensions.ProjectGetProjectsForMember(connection, member.Id);

			var projectStats = new List<ProjectStatsView>();

			foreach(var project in projects)
			{
				projectStats.Add(new ProjectStatsView
				{
					Id = project.Id,
					Name = project.Name
				});
			}

			return new()
			{
				UserId = member.Id,
				ProjectStats = projectStats
			};
		}
	}
}