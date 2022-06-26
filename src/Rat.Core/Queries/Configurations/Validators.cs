using System;
using System.Collections.Generic;
using Rat.Core.Properties;

namespace Rat.Core.Queries.Configurations
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateProjectId(int projectId)
		{
			if (projectId <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("ProjectId", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}
	}
}
