using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Commands.Users.CreateUser;
using Rat.DataAccess;

namespace Rat.Queries.Users.GetUserByUserId
{
	internal class GetUserByUserIdQuery : IRequestHandler<GetUserByUserIdRequest, GetUserByUserIdResponse>
	{
		private const string SQL_QUERY = "SELECT Id FROM User WHERE UserId = @UserId";

		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public GetUserByUserIdQuery(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
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

			var createUserResponse = await _mediator.Send(new CreateUserRequest { AuthProviderUserId = request.AuthProviderUserId });

			return new() { Id = createUserResponse.Id };
		}
	}
}
