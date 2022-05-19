using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class MemberProjectSqlConnectionExtensions
	{
		private const string MemberIdParameter = "@memberId";
		private const string ProjectIdParameter = "@projectId";

		public async static Task MemberProjectInsert(
			this SqlConnection connection,
			int memberId,
			int projectId,
			int createdBy,
			CancellationToken ct)
		{
			_ = await Insert(connection, memberId, projectId, createdBy, ct);
		}

		internal async static Task<int> Insert(
			SqlConnection connection,
			int memberId,
			int projectId,
			int createdBy,
			CancellationToken ct)
		{
			const string ProcedureName = "MemberProject_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(MemberIdParameter, memberId);
			parameters.Add(ProjectIdParameter, projectId);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}

		public async static Task MemberProjectDelete(
			this SqlConnection connection,
			int memberId,
			int projectId,
			int deletedBy,
			CancellationToken ct)
		{
			_ = await Delete(connection, memberId, projectId, deletedBy, ct);
		}

		internal async static Task<int> Delete(
			SqlConnection connection,
			int memberId,
			int projectId,
			int deletedBy,
			CancellationToken ct)
		{
			const string ProcedureName = "MemberProject_Delete";

			var parameters = new DynamicParameters();
			parameters.Add(MemberIdParameter, memberId);
			parameters.Add(ProjectIdParameter, projectId);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}
	}
}
