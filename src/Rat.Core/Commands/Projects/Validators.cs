using System;
using System.Collections.Generic;
using Rat.Core.Properties;
using static Rat.Sql.DatabaseSchema;

namespace Rat.Commands.Projects
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return new KeyValuePair<string, string>[1] { new("Name", Resources.MustNotBeNullOrEmpty) };
			}

			if (name.Length > ProjectSchema.Max_Name_Length)
			{
				return new KeyValuePair<string, string>[1]
				{
					new ("Name", string.Format(Resources.MaximumLengthLimitExceeded, name.Length, ProjectSchema.Max_Name_Length))
				};
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateAuthProviderUserId(string authProviderUserId)
		{
			if (string.IsNullOrWhiteSpace(authProviderUserId))
			{
				return new KeyValuePair<string, string>[1] { new("AuthProviderId", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateProjectTypeId(int projectTypeId)
		{
			if (projectTypeId <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("ProjectTypeId", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}
	}
}
