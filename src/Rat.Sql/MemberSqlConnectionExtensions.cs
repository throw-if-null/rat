using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class MemberSqlConnectionExtensions
	{
		private const string AuthProviderIdParameter = "@authProviderId";

		public async static Task<(int Id, int NoC)> MemberInsert(
			this SqlConnection connection,
			string authProviderId,
			int createdBy)
		{
			const string ProcedureName = "Member_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(AuthProviderIdParameter, authProviderId);
			parameters.AddCreatedBy(createdBy);

			var id = await connection.QuerySingleEx<int>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			return (id, noc);
		}

		public async static Task<dynamic> MemberGetByAuthProviderId(
			this SqlConnection connection,
			string authProviderId)
		{
			const string ProcedureName = "Member_GetByAuthProviderId";

			var parameters = new DynamicParameters();
			parameters.Add(AuthProviderIdParameter, authProviderId);
			parameters.AddNoc();

			var projectType = await connection.QuerySingleEx<dynamic>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			if (noc != 1)
				throw new DatabaseException($"Expected number of rows: {noc} is not 1");

			return projectType;
		}

		public async static Task<int> Member_SoftDelete(
			this SqlConnection connection,
			int id,
			int modifiedBy)
		{
			const string ProcedureName = "Member_SoftDelete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddModifiedBy(modifiedBy);

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}
	}
}
