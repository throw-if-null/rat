using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class ProjectSqlConnectionExtensions
	{
		private const string NameParameter = "@name";
		private const string ProjectTypeIdParameter = "@projectTypeId";
		private const string MemberIdParameter = "@memberId";

		public async static Task<dynamic> ProjectInsert(
			this SqlConnection connection,
			string name,
			int projectTypeId,
			int createdBy,
			CancellationToken ct)
		{
			var (project, _) = await Insert(connection, name, projectTypeId, createdBy, ct);

			return project;
		}

		internal async static Task<(dynamic Project, int NumberOfChanges)> Insert(
			SqlConnection connection,
			string name,
			int projectTypeId,
			int createdBy,
			CancellationToken ct)
		{
			const string ProcedureName = "Project_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(NameParameter, name);
			parameters.Add(ProjectTypeIdParameter, projectTypeId);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var project = await connection.QuerySingleEx<dynamic>(ProcedureName, parameters, ct);
			var numberOfChanges = parameters.GetNoc();

			return (project, numberOfChanges);
		}

		public async static Task ProjectUpdate(
			this SqlConnection connection,
			string name,
			int? projectTypeId,
			int id,
			int modifiedBy,
			CancellationToken ct)
		{
			_ = await Update(connection, name, projectTypeId, id, modifiedBy, ct);
		}

		internal async static Task<int> Update(
			SqlConnection connection,
			string name,
			int? projectTypeId,
			int id,
			int modifiedBy,
			CancellationToken ct)
		{
			const string ProcedureName = "Project_Update";

			var parameters = new DynamicParameters();
			parameters.Add(NameParameter, name);
			parameters.Add(ProjectTypeIdParameter, projectTypeId);
			parameters.AddModifiedBy(modifiedBy);
			parameters.AddId(id);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}

		public async static Task<dynamic> ProjectGetById(
			this SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			var (project, _) = await GetById(connection, id, ct);

			return project;
		}

		internal async static Task<(dynamic Project, int NumberOfChanges)> GetById(
			SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			const string ProcedureName = "Project_GetById";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddNoc();

			var project = await connection.QuerySingleOrDefaultEx<dynamic>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (project, noc);
		}

		public async static Task<IEnumerable<dynamic>> ProjectGetProjectsForMember(
			this SqlConnection connection,
			int memberId,
			CancellationToken ct)
		{
			var (projects, _) = await GetProjectsForMember(connection, memberId, ct);

			return projects;
		}

		internal async static Task<(dynamic Projects, int NumberOfChanges)> GetProjectsForMember(
			SqlConnection connection,
			int memberId,
			CancellationToken ct)
		{
			const string ProcedureName = "Project_GetProjectsForMember";

			var parameters = new DynamicParameters();
			parameters.Add(MemberIdParameter, memberId);
			parameters.AddNoc();

			var projects = await connection.QueryEx<dynamic>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (projects, noc);
		}

		public async static Task ProjectDelete(
			this SqlConnection connection,
			int id,
			int deletedBy,
			CancellationToken ct)
		{
			_ = await Delete(connection, id, deletedBy, ct);
		}

		internal async static Task<int> Delete(
			SqlConnection connection,
			int id,
			int deletedBy,
			CancellationToken ct)
		{
			const string ProcedureName = "Project_Delete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}
	}
}
