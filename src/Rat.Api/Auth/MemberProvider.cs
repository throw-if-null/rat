using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rat.Sql;

namespace Rat.Api.Auth
{
	public sealed class MemberProvider : IMemberProvider
	{
		private static readonly string Method = $"{nameof(MemberProvider)}.{nameof(GetMemberId)}";

		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly ILogger _logger;

		public MemberProvider(
			IHttpContextAccessor contextAccessor,
			ISqlConnectionFactory connectionFactory,
			ILogger<MemberProvider> logger)
		{
			_contextAccessor = contextAccessor;
			_connectionFactory = connectionFactory;
			_logger = logger;
		}

		public async Task<int> GetMemberId(CancellationToken ct)
		{
			ClaimsPrincipal user = _contextAccessor.HttpContext.User;

			using var methodScope = _logger.AppendMethodScope(Method);
			_logger.ProcessingStartedDebug();

			if (user == null)
			{
				_logger.UserInHttpContexIsNullWarning();

				return default;
			}

			if (user.Identity == null)
			{
				_logger.IdentityInUserIsNullWarning();

				return default;
			}

			var name = user.Identity.Name;

			if (string.IsNullOrWhiteSpace(name))
			{
				var nameIdentifierClaim = user.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

				if (nameIdentifierClaim == null)
				{
					_logger.NameClaimIsNullOrEmptyWarning();

					return default;
				}

				using var clientNameClaimScope = _logger.AppendClientNameClaim(nameIdentifierClaim.Value);

				if (nameIdentifierClaim.Value.EndsWith("@clients", StringComparison.InvariantCultureIgnoreCase))
				{
					var clientName = nameIdentifierClaim.Value.Split('@')[0];

					return 1;
				}
				else
				{
					_logger.NameClientClaimIsInvalidWarning();

					return default;
				}
			}

			using var nameClaimScope = _logger.AppendNameClaim(name);

			if (!name.Contains('|'))
			{
				_logger.NameClaimShouldContainPipeWarning();

				return default;
			}

			var authProviderId = name.Split('|')[1];

			using var userIdScope = _logger.AppendUserId(authProviderId);
			_logger.ProcessingFinishedDebug();

			await using var connection = _connectionFactory.CreateConnection();

			var member = await connection.MemberGetByAuthProviderId(authProviderId, ct);

			if (member == null)
				return default;

			return member.Id;
		}
	}
}
