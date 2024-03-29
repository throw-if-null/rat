﻿using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class ProjectTypeSqlConnectionExtensions
	{
		private const string NameParameter = "@name";

		public async static Task<(int Id, int NumberOfChanges)> ProjectTypeInsert(
			this SqlConnection connection,
			string name,
			int createdBy,
			CancellationToken ct)
		{
			const string ProcedureName = "ProjectType_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(NameParameter, name);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var id = await connection.QuerySingleEx<int>(ProcedureName, parameters, ct);
			var numberOfChanges = parameters.GetNoc();

			return (id, numberOfChanges);
		}

		public async static Task<int> ProjectTypeUpdate(
			this SqlConnection connection,
			int id,
			string name,
			int modifiedBy,
			CancellationToken ct)
		{
			const string ProcedureName = "ProjectType_Update";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.Add(NameParameter, name);
			parameters.AddModifiedBy(modifiedBy);
			parameters.AddNoc();

			var numberOfChanges = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return numberOfChanges;
		}

		public async static Task<dynamic> ProjectTypeGetById(
			this SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			var (projectType, _) = await GetById(connection, id, ct);

			return projectType;
		}

		internal async static Task<(dynamic ProjectType, int Noc)> GetById(
			SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			const string ProcedureName = "ProjectType_GetById";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddNoc();

			var projectType = await connection.QuerySingleEx<dynamic>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (projectType, noc);
		}

		public async static Task<(dynamic ProjecTypes, int NumberOfChanges)> ProjectTypeGetAll(
			this SqlConnection connection,
			CancellationToken ct)
		{
			const string ProcedureName = "ProjectType_GetAll";

			var parameters = new DynamicParameters();
			parameters.AddNoc();

			var projectTypes = await connection.QueryEx<dynamic>(ProcedureName, parameters, ct);
			var numberOfChanges = parameters.GetNoc();

			return (projectTypes, numberOfChanges);
		}

		public async static Task<int> ProjectTypeDelete(
			this SqlConnection connection,
			int id,
			int deleteBy,
			CancellationToken ct)
		{
			const string ProcedureName = "ProjectType_Delete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddDeletedBy(deleteBy);
			parameters.AddNoc();

			var numberOfChanges = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return numberOfChanges;
		}
	}
}
