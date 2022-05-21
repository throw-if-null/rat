using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Queries.Projects;
using Rat.Sql;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal class GetProjectsForUserQuery : IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public GetProjectsForUserQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetProjectsForUserResponse> Handle(GetProjectsForUserRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var projects = await ProjectSqlConnectionExtensions.ProjectGetProjectsForMember(connection, request.MemberId, cancellationToken);

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
				UserId = request.MemberId,
				ProjectStats = projectStats
			};
		}
	}
}