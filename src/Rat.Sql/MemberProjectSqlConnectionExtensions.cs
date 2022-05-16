using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	internal static class MemberProjectSqlConnectionExtensions
	{
		private const string MemberIdParameter = "@memberId";
		private const string ProjectIdParameter = "@projectId";

		public async static Task<int> MemberProjectInsert(
			this SqlConnection connection,
			int memberId,
			int projectId,
			int createdBy)
		{
			const string ProcedureName = "MemberProject_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(MemberIdParameter, memberId);
			parameters.Add(ProjectIdParameter, projectId);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}

		public async static Task<int> MemberProjectDelete(
			this SqlConnection connection,
			int memberId,
			int projectId,
			int deletedBy)
		{
			const string ProcedureName = "MemberProject_Delete";

			var parameters = new DynamicParameters();
			parameters.Add(MemberIdParameter, memberId);
			parameters.Add(ProjectIdParameter, projectId);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}
	}
}
