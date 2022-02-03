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

            using var _ = _logger.AppendMethodScope(Method);
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
            using var __ = _logger.AppendNameClaim(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.NameClaimIsNullOrEmptyWarning();

                return null;
            }

            if (!name.Contains('|'))
            {
                _logger.NameClaimShouldContainPipe();

                return null;
            }

            var userId = name.Split('|')[1];

            using var ___ = _logger.AppendUserId(userId);
            _logger.ProcessingFinishedDebug();

            return userId;
        }
    }
}
