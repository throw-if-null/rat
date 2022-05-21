using System;
using System.Collections.Generic;
using Rat.Core.Properties;

namespace Rat.Core.Commands
{
	internal static class Validators
	{
		private static readonly KeyValuePair<string, string>[] Empty = Array.Empty<KeyValuePair<string, string>>();

		public static KeyValuePair<string, string>[] ValidateId(int id)
		{
			if (id <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("Id", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}

		internal static IEnumerable<KeyValuePair<string, string>> ValidateCreatedBy(int createdBy)
		{
			if (createdBy <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("CreatedBy", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}

		internal static IEnumerable<KeyValuePair<string, string>> ValidateModifiedBy(int modifiedBy)
		{
			if (modifiedBy <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("ModifiedBy", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}

		internal static IEnumerable<KeyValuePair<string, string>> ValidateDeletedBy(int deletedBy)
		{
			if (deletedBy <= 0)
			{
				return new KeyValuePair<string, string>[1] { new("DeletedBy", Resources.IdMustBeLargerThenZero) };
			}

			return Empty;
		}
	}
}
