using System;
using System.Collections.Generic;
using Rat.Core.Commands.ConfigurationEntries.PatchConfigurationEntry;
using Rat.Core.Properties;
using static Rat.Sql.DatabaseSchema;

namespace Rat.Core.Commands.ConfigurationEntries
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return new KeyValuePair<string, string>[1] { new("Name", Resources.MustNotBeNullOrEmpty) };
			}

			return ValidateKeyMaxLenghth(key);
		}

		public static KeyValuePair<string, string>[] ValidateKeyMaxLenghth(string key)
		{
			if (key?.Length > ConfigurationEntrySchema.Max_Key_Length)
			{
				return new KeyValuePair<string, string>[1]
				{
					new ("Key", string.Format(Resources.MaximumLengthLimitExceeded, key.Length, ConfigurationEntrySchema.Max_Key_Length))
				};
			}

			return Empty;
		}

		internal static KeyValuePair<string, string>[] AllNull(PatchConfigurationEntryRequest request)
		{
			var allNull =
				request.Key == null &&
				request.Value == null &&
				request.SecondsToLive == null &&
				request.Disabled == null;

			if (allNull)
			{
				return new KeyValuePair<string, string>[1] { new("Request", "Request data contains no changes") };
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateValue(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return new KeyValuePair<string, string>[1] { new("Name", Resources.MustNotBeNullOrEmpty) };
			}

			return ValidateValueMaxLenght(value);
		}

		public static KeyValuePair<string, string>[] ValidateValueMaxLenght(string value)
		{
			if (value?.Length > ConfigurationEntrySchema.Max_Value_Length)
			{
				return new KeyValuePair<string, string>[1]
				{
					new ("Value", string.Format(Resources.MaximumLengthLimitExceeded, value.Length, ConfigurationEntrySchema.Max_Value_Length))
				};
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateConfigurationRootId(int configurationRootId)
		{
			if (configurationRootId <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("ConfigurationTypeId", Resources.NotFound) };
			}

			return Empty;
		}

		public static KeyValuePair<string, string>[] ValidateSecondsToLive(int secondsToLive)
		{
			if (secondsToLive < -1)
			{
				return new KeyValuePair<string, string>[1] { new("SecondstoLive", Resources.NotFound) };
			}

			return Empty;
		}
	}
}
