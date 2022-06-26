using System;
using System.Collections.Generic;
using Rat.Core.Properties;

namespace Rat.Commands.Users
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateAuthProviderId(string authProviderUserId)
		{
			if (string.IsNullOrWhiteSpace(authProviderUserId))
			{
				return new KeyValuePair<string, string>[1]
				{
					new KeyValuePair<string, string>("AuthProviderId", Resources.MustNotBeNullOrEmpty)
				};
			}

			return Empty;
		}
	}
}
