using Rat.Core;
using Rat.Queries.Projects.GetProjectById;
using Rat.Queries.Projects.GetProjectsForUser;
using Rat.Queries.Properties;

namespace Rat.Queries.Projects
{
	internal static class Validators
	{
		public static void ValidateId(int id, RatContext context)
		{
			if (id <= 0)
			{
				context.ValidationErrors.Add(
					GetProjectByIdRequest.ID_SIGNATURE,
					Resources.IdMustBeLargerThenZero);
			}
		}

		public static void MakeGoodOrBad(RatContext context)
		{
			context.Status = ProcessingStatus.GoodRequest;

			if (context.ValidationErrors.Count > 0)
			{
				context.Status = ProcessingStatus.BadRequest;
				context.FailureReason = Resources.BadRequest;
			}
		}

		public static void ValidateUserId(string userId, RatContext context)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				context.ValidationErrors.Add(
					GetProjectsForUserRequest.UserId_Signature,
					Resources.MustNotBeNullOrEmpty);
			}
		}
	}
}
