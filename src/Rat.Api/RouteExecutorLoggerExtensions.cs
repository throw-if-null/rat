using Rat.Core.Observability.Logging;

namespace Rat.Api
{
	internal static class RouteExecutorLoggerExtensions
	{
		private static readonly Func<EventId> _getMeasureExecutionTimeEvent = () => new EventId(LogEvents.MeasureExecutionTime, nameof(LogEvents.MeasureExecutionTime));
		private static readonly Func<EventId> _getBadRequestEvent = () => new EventId(LogEvents.BadRequest, nameof(LogEvents.BadRequest));
		private static readonly Func<EventId> _getResourceNotFoundEvent = () => new EventId(LogEvents.ResourceNotFound, nameof(LogEvents.ResourceNotFound));
		private static readonly Func<EventId> _getDatabaseFailureEvent = () => new EventId(LogEvents.DatabaseFailure, nameof(LogEvents.DatabaseFailure));
		private static readonly Func<EventId> _getUnhandledExceptionEvent = () => new EventId(LogEvents.UnhandledException, nameof(LogEvents.UnhandledException));

		private static readonly Func<ILogger, string, IDisposable> _appendRouteName = LoggerMessage.DefineScope<string>("{RouteName}");
		private static readonly Func<ILogger, double, IDisposable> _appendElapsedTimeInMs = LoggerMessage.DefineScope<double>("{ElapsedTimeInMs}");

		private static Func<EventId, Action<ILogger, string, Exception>> Information => (EventId id) =>
			LoggerMessage.Define<string>(
				LogLevel.Information,
				id,
				"{Message}");

		private static Func<EventId, Action<ILogger, string, Exception>> Warning => (EventId id) =>
			LoggerMessage.Define<string>(
				LogLevel.Warning,
				id,
				"{Message}");

		private static Func<EventId, Action<ILogger, string, Exception>> Error => (EventId id) =>
			LoggerMessage.Define<string>(
				LogLevel.Error,
				id,
				"{Message}");

		private static Func<EventId, Action<ILogger, string, Exception>> Critical => (EventId id) =>
			LoggerMessage.Define<string>(
				LogLevel.Critical,
				id,
				"{Message}");

		public static IDisposable AppendRouteNameScope(this ILogger logger, string routeName) =>  _appendRouteName(logger, routeName);
		public static IDisposable AppendElapsedTimeInMsScope(this ILogger logger, double elapsedTimeInMs) => _appendElapsedTimeInMs(logger, elapsedTimeInMs);

		public static void FinishedExecutingRouteInfo(this ILogger logger) => Information(_getMeasureExecutionTimeEvent())(logger, "Finished executing route", default!);
		public static void BadRequestWarning(this ILogger logger, Exception exception) => Warning(_getBadRequestEvent())(logger, "Bad request", exception);
		public static void ResourceNotFoundWarning(this ILogger logger, Exception exception) => Warning(_getResourceNotFoundEvent())(logger, "Resource not found", exception);
		public static void DatabaseFailureError(this ILogger logger, Exception exception) => Error(_getDatabaseFailureEvent())(logger, "Database error", exception);
		public static void UnhandledExceptionCritical(this ILogger logger, Exception exception) => Critical(_getUnhandledExceptionEvent())(logger, "Unhandled error", exception);
	}
}
