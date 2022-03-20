using Dapper;
using MediatR;
using Rat.Data.Exceptions;
using Rat.DataAccess;

namespace Rat.Queries.Users.GetUserByUserId
{
	internal class GetUserByUserIdQuery : IRequestHandler<GetUserByUserIdRequest, GetUserByUserIdResponse>
	{
		private const string SQL_QUERY = "SELECT Id FROM User WHERE UserId = @UserId";

		private readonly ISqlConnectionFactory _connectionFactory;

		public GetUserByUserIdQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetUserByUserIdResponse> Handle(GetUserByUserIdRequest request, CancellationToken cancellationToken)
		{
			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(
				SQL_QUERY,
				new { AuthProviderUserId = request.AuthProviderUserId },
				cancellationToken: cancellationToken);

			var id = await connection.QuerySingleOrDefaultAsync<int?>(command);

			if (id.HasValue)
				return new() { Id = id.Value };

			throw new ResourceNotFoundException($"User: {request.AuthProviderUserId} doesn't exist");
		}
	}
}
