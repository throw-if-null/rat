using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.DataAccess;

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

			var command = new CommandDefinition(
				SqlQuery,
				new { Id = request.Id },
				cancellationToken: cancellationToken);

			var id = await connection.QuerySingleOrDefaultAsync<int?>(command);

			return id.HasValue
				? new() { Id = id.Value }
				: null;
		}
	}
}
