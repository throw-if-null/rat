using Rat.Core;

namespace Rat.Api.Routes
{
	internal static class HttpResponseHandler
	{
		public static IResult HandleUnscusseful(RatContext context)
		{
			switch (context.Status)
			{
				case ProcessingStatus.BadRequest:
					return Results.BadRequest(context.ValidationErrors);

				case ProcessingStatus.NotFound:
					return Results.NotFound();

				default:
					return Results.Problem(context.FailureReason);
			}
		}
	}
}
