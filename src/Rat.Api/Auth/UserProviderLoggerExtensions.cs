using Rat.Core.Observability.Logging;

namespace Rat.Api.Auth
{
	internal static class UserProviderLoggerExtensions
	{
		private static readonly Func<EventId> _getEvent = () => new EventId(LogEvents.ExtractUserIdFromHttpContext, nameof(LogEvents.ExtractUserIdFromHttpContext));

		private static readonly Func<ILogger, string, IDisposable> _appendMethod = LoggerMessage.DefineScope<string>("{Method}");
		private static readonly Func<ILogger, string, IDisposable> _appendNameClaim = LoggerMessage.DefineScope<string>("{NameClaim}");
		private static readonly Func<ILogger, string, IDisposable> _appendClientNameClaim = LoggerMessage.DefineScope<string>("{ClientNameClaim}");
		private static readonly Func<ILogger, string, IDisposable> _appendUserId = LoggerMessage.DefineScope<string>("{UserId}");

		private static readonly Action<ILogger, string, Exception> _debug =
			LoggerMessage.Define<string>(
				LogLevel.Debug,
				_getEvent(),
				"{Message}");

		private static readonly Action<ILogger, string, Exception> _warning =
			LoggerMessage.Define<string>(
				LogLevel.Warning,
				_getEvent(),
				"{Message}");

		public static IDisposable AppendMethodScope(this ILogger logger, string method) => _appendMethod(logger, method);
		public static IDisposable AppendNameClaim(this ILogger logger, string nameClaim) => _appendNameClaim(logger, nameClaim);
		public static IDisposable AppendClientNameClaim(this ILogger logger, string clientNameClaim) => _appendClientNameClaim(logger, clientNameClaim);
		public static IDisposable AppendUserId(this ILogger logger, string userId) => _appendUserId(logger, userId);

		public static void ProcessingStartedDebug(this ILogger logger) => _debug(logger, "Processing started", default!);
		public static void ProcessingFinishedDebug(this ILogger logger) => _debug(logger, "Processing finished", default!);

		public static void UserInHttpContexIsNullWarning(this ILogger logger) => _warning(logger, "HttpContext.User is null", default!);
		public static void IdentityInUserIsNullWarning(this ILogger logger) => _warning(logger, "HttpContext.User.Identity is null", default!);
		public static void NameClaimIsNullOrEmptyWarning(this ILogger logger) => _warning(logger, "Identity.Name is null/empty. Check the sub claim", default!);
		public static void NameClaimShouldContainPipeWarning(this ILogger logger) => _warning(logger, "Identity.Name should contain |", default!);
		public static void UnknownClientNameWarning(this ILogger logger) => _warning(logger, "Unknown client", default);
		public static void NameClientClaimIsInvalidWarning(this ILogger logger) => _warning(logger, "Client claim name value is invalid", default);
	}
}
