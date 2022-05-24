using System;
using System.Collections.Generic;
using Rat.Core.Properties;
using Rat.Queries.Projects.GetProjectById;
using Rat.Queries.Projects.GetProjectsForUser;

namespace Rat.Queries.Projects
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateId(int id)
		{
			if (id <= 0)
			{
				return new KeyValuePair<string, string>[1]
				{
					new (GetProjectByIdRequest.ID_SIGNATURE, Resources.IdMustBeLargerThenZero)
				};
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateUserId(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				return new KeyValuePair<string, string>[1]
				{
					new KeyValuePair<string, string>(GetProjectsForUserRequest.UserId_Signature, Resources.MustNotBeNullOrEmpty)
				};
			}

			return Empty;
		}
	}
}
