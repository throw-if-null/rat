namespace Rat.Core.Observability.Logging
{
    public static class LogEvents
    {
		public const int MeasureExecutionTime = 1000;
        public const int ExtractUserIdFromHttpContext = 10000;

		public const int BadRequest = 11100;
		public const int ResourceNotFound = 11200;
		public const int DatabaseFailure = 11300;
		public const int UnhandledException = 1400;
	}
}
