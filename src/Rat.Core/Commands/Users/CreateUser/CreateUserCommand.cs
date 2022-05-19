using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Sql;

namespace Rat.Commands.Users.CreateUser
{
	internal class CreateUserCommand : IRequestHandler<CreateUserRequest, CreateUserResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public CreateUserCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();
			var id = await connection.MemberInsert(request.AuthProviderId, 1, cancellationToken);

			return new() { Id = id };
		}
	}
}
