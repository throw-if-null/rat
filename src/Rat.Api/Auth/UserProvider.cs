using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Rat.Api.Auth
{
	public sealed class UserProvider : IUserProvider
	{
		private static readonly string Method = $"{nameof(UserProvider)}.{nameof(GetUserId)}";

		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ILogger _logger;

		public UserProvider(IHttpContextAccessor contextAccessor, ILogger<UserProvider> logger)
		{
			_contextAccessor = contextAccessor;
			_logger = logger;
		}

		public string GetUserId()
		{
			var user = _contextAccessor.HttpContext.User;

			using var methodScope = _logger.AppendMethodScope(Method);
			_logger.ProcessingStartedDebug();

			if (user == null)
			{
				_logger.UserInHttpContexIsNullWarning();

				return null;
			}

			if (user.Identity == null)
			{
				_logger.IdentityInUserIsNullWarning();

				return null;
			}

			var name = user.Identity.Name;

			if (string.IsNullOrWhiteSpace(name))
			{
				var nameIdentifierClaim = user.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

				if (nameIdentifierClaim == null)
				{
					_logger.NameClaimIsNullOrEmptyWarning();

					return null;
				}

				using var clientNameClaimScope = _logger.AppendClientNameClaim(nameIdentifierClaim.Value);

				if (nameIdentifierClaim.Value.EndsWith("@clients", StringComparison.InvariantCultureIgnoreCase))
				{
					var clientName = nameIdentifierClaim.Value.Split('@')[0];

					return clientName;
				}
				else
				{
					_logger.NameClientClaimIsInvalidWarning();

					return null;
				}
			}

			using var nameClaimScope = _logger.AppendNameClaim(name);

			if (!name.Contains('|'))
			{
				_logger.NameClaimShouldContainPipeWarning();

				return null;
			}

			var userId = name.Split('|')[1];

			using var userIdScope = _logger.AppendUserId(userId);
			_logger.ProcessingFinishedDebug();

			return userId;
		}
	}
}
