using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Sql;

namespace Rat.Core.Queries.ProjectTypes
{
	internal class GetProjectTypeByIdQuery : IRequestHandler<GetProjectTypeByIdRequest, GetProjectTypeByIdResponse>
	{
		private const string SqlQuery = "SELECT Id FROM ProjectType WHERE Id = @Id";

		private readonly ISqlConnectionFactory _connectionFactory;

		public GetProjectTypeByIdQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}
		public async Task<GetProjectTypeByIdResponse> Handle(GetProjectTypeByIdRequest request, CancellationToken cancellationToken)
		{
			await using var connection = _connectionFactory.CreateConnection();
			var projectType = await connection.ProjectTypeGetById(request.Id, cancellationToken);

			return projectType != null
				? new() { Id = projectType.Id }
				: null;
		}
	}
}
