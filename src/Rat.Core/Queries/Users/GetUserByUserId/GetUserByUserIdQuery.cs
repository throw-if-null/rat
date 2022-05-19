using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Sql;

namespace Rat.Queries.Users.GetUserByUserId
{
	internal class GetUserByUserIdQuery : IRequestHandler<GetUserByUserIdRequest, GetUserByUserIdResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public GetUserByUserIdQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetUserByUserIdResponse> Handle(GetUserByUserIdRequest request, CancellationToken cancellationToken)
		{
			await using var connection = _connectionFactory.CreateConnection();
			var member = await connection.MemberGetByAuthProviderId(request.AuthProviderId, cancellationToken);

			if (member != null)
				return new() { Id = member.Id };

			var id = await connection.MemberInsert(request.AuthProviderId, 1, cancellationToken);

			return new() { Id = id };
		}
	}
}
