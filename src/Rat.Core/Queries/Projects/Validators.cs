using System;
using System.Collections.Generic;
using Rat.Core.Properties;

namespace Rat.Queries.Projects
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateMemberId(int memberId)
		{
			if (memberId <= 0)
			{
				return new KeyValuePair<string, string>[1]
				{
					new KeyValuePair<string, string>("MemberId", Resources.IdMustBeLargerThenZero)
				};
			}

			return Empty;
		}
	}
}
