using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rat.Core.Observability.Logging;

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

            using var _ = _logger.BeginScope("{Method}", Method);

            if (user == null)
            {
                _logger.LogWarning(LogEvents.ExtractUserIdFromHttpContext, "HttpContext.User is null");

                return null;
            }

            if (user.Identity == null)
            {
                _logger.LogWarning(LogEvents.ExtractUserIdFromHttpContext, "HttpContext.User.Identity is null");

                return null;
            }

            var name = user.Identity.Name;
            using var __ = _logger.BeginScope("{NameClaim}", name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning(LogEvents.ExtractUserIdFromHttpContext, "Identity.Name is null/empty. Check the sub claim");

                return null;
            }

            if (!name.Contains('|'))
            {
                _logger.LogWarning(
                    LogEvents.ExtractUserIdFromHttpContext,
                    "Identity.Name should contain |", name);

                return null;
            }

            var userId = name.Split('|')[1];

            using var ___ = _logger.BeginScope("{UserId}", userId);
            _logger.LogInformation("{Method} finished");

            return userId;
        }
    }
}
