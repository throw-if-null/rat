using Rat.Core.Observability;
using Rat.Data.Exceptions;

namespace Rat.Api
{
	public class RouteExecutor
	{
		private readonly ILogger _logger;

		public RouteExecutor(ILogger<RouteExecutor> logger)
		{
			_logger = logger;
		}

		public async Task<IResult> Execute<T>(string routeName, Func<Task<T>> routeProcess, Func<T, IResult> successfulResponse)
		{
			var watch = ValueStopwatch.StartNew();

			using var routeNameScope = _logger.AppendRouteNameScope(routeName);

			try
			{
				var response = await routeProcess();

				using var elapsedTimeInMsScope = _logger.AppendElapsedTimeInMsScope(watch.GetElapsedTime().TotalMilliseconds);
				_logger.FinishedExecutingRouteInfo();

				return successfulResponse(response);
			}
			catch (InvalidRequestDataException ex)
			{
				_logger.BadRequestWarning(ex);

				return Results.BadRequest(ex.Data["ValidationErrors"]);
			}
			catch(ResourceNotFoundException ex)
			{
				_logger.ResourceNotFoundWarning(ex);

				return Results.NotFound();
			}
			catch(RatDbException ex)
			{
				_logger.DatabaseFailureError(ex);

				return Results.Problem("Database error");
			}
			catch(Exception ex)
			{
				_logger.UnhandledExceptionCritical(ex);

				return Results.Problem("Unhandled exception");
			}
		}
	}
}
