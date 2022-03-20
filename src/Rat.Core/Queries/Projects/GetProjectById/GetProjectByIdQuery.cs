using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.DataAccess;
using Rat.DataAccess.Entities;

namespace Rat.Queries.Projects.GetProjectById
{
	internal class GetProjectByIdQuery : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
	{
		private const string SQL_QUERY = "SELECT Id, Name, ProjectTypeId FROM Project WHERE Id = @Id";

		private readonly ISqlConnectionFactory _connectionFactory;

		public GetProjectByIdQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetProjectByIdResponse> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var project = await connection.QuerySingleOrDefaultAsync<ProjectEntity>(SQL_QUERY, cancellationToken);

			return project == null
				? null
				: new()
				{
					Id = project.Id,
					Name = project.Name,
					TypeId = project.ProjectTypeId
				};
		}
	}
}
