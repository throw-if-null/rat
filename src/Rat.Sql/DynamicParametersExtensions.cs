using System.Data;
using Dapper;

namespace Rat.Sql
{
	internal static class DynamicParametersExtensions
	{
		internal const string IdParameter = "@id";
		internal const string CreatedByParameter = "@createdBy";
		internal const string ModifiedByParameter = "@modifiedBy";
		internal const string DeletedByParameter = "@deletedBy";
		internal const string NocParameter = "numberOfChanges";

		public static DynamicParameters AddId(this DynamicParameters parameters, int id)
		{
			parameters.Add(IdParameter, id);

			return parameters;
		}

		public static DynamicParameters AddCreatedBy(this DynamicParameters parameters, int createdBy)
		{
			parameters.Add(CreatedByParameter, createdBy);

			return parameters;
		}

		public static DynamicParameters AddModifiedBy(this DynamicParameters parameters, int modifiedBy)
		{
			parameters.Add(ModifiedByParameter, modifiedBy);

			return parameters;
		}

		public static DynamicParameters AddDeletedBy(this DynamicParameters parameters, int deletedBy)
		{
			parameters.Add(DeletedByParameter, deletedBy);

			return parameters;
		}

		public static DynamicParameters AddNoc(this DynamicParameters parameters)
		{
			parameters.Add(NocParameter, DbType.Int32, direction: ParameterDirection.Output);

			return parameters;
		}

		public static int GetNoc(this DynamicParameters parameters)
		{
			return parameters.Get<int>(NocParameter);
		}
	}
}
