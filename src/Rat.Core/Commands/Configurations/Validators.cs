using System;
using System.Collections.Generic;
using Rat.Core.Properties;
using static Rat.Sql.DatabaseSchema;

namespace Rat.Core.Commands.Configurations
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

		public static KeyValuePair<string, string>[] ValidateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return new KeyValuePair<string, string>[1] { new("Name", Resources.MustNotBeNullOrEmpty) };
			}

			if (name.Length > ConfigurationRootSchema.Max_Name_Length)
			{
				return new KeyValuePair<string, string>[1]
				{
					new ("Name", string.Format(Resources.MaximumLengthLimitExceeded, name.Length, ConfigurationRootSchema.Max_Name_Length))
				};
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateConfigurationTypeId(int configurationTypeId)
		{
			if (configurationTypeId <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("ConfigurationTypeId", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}
	}
}
