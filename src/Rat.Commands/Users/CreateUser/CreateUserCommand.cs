using Dapper;
using MediatR;
using Rat.DataAccess;

namespace Rat.Commands.Users.CreateUser
{
	internal class CreateUserCommand : IRequestHandler<CreateUserRequest, CreateUserResponse>
	{
		private const string SqlQuery = "";

		private readonly ISqlConnectionFactory _connectionFactory;

		public CreateUserCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(
				SqlQuery,
				new { AuthProviderUserId = request.AuthProviderUserId },
				cancellationToken: cancellationToken);

			var id = await connection.QuerySingleAsync<int>(command);

			return new() { Id = id };
		}
	}
}
