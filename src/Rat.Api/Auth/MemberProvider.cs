﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
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
			try
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

				var authProviderId = string.Empty;
				var nameIdentifierClaim = user.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

				if (nameIdentifierClaim == null)
				{
					_logger.NameClaimIsNullOrEmptyWarning();

					return default;
				}

				using var clientNameClaimScope = _logger.AppendClientNameClaim(nameIdentifierClaim.Value);

				if (nameIdentifierClaim.Value.EndsWith("@clients", StringComparison.InvariantCultureIgnoreCase))
				{
					authProviderId = nameIdentifierClaim.Value.Split('@')[0];
				}
				else if (nameIdentifierClaim.Value.Contains('|', StringComparison.InvariantCultureIgnoreCase))
				{
					authProviderId = nameIdentifierClaim.Value.Split('|')[1];
				}

				if (!string.IsNullOrWhiteSpace(authProviderId))
				{
					using var userIdScope = _logger.AppendUserId(authProviderId);

					await using var connection = _connectionFactory.CreateConnection();
					var memberId = await GetMemberId(connection, authProviderId, ct);

					if (memberId == default)
						memberId = await connection.MemberInsert(authProviderId, 1, ct);

					return memberId;
				}

				_logger.NameClientClaimIsInvalidWarning();

				return default;
			}
			finally
			{
				_logger.ProcessingFinishedDebug();
			}
		}

		private async static Task<int> GetMemberId(SqlConnection connection, string authProviderId, CancellationToken ct)
		{
			if (connection is null) throw new ArgumentNullException(nameof(connection));
			if (string.IsNullOrEmpty(authProviderId)) throw new ArgumentException($"'{nameof(authProviderId)}' cannot be null or empty.", nameof(authProviderId));
			var member = await connection.MemberGetByAuthProviderId(authProviderId, ct);

			if (member == null)
				return default;

			return member.Id;
		}
	}
}
