using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class ConfigurationRootSqlConnectionExtensions
	{
		private const string NameParameter = "@name";
		private const string ConfigurationTypeIdParameter = "@configurationTypeId";
		private const string ProjectIdParameter = "@projectId";

		public async static Task<int> ConfigurationRootInsert(
			this SqlConnection connection,
			int projectId,
			string name,
			int configurationTypeId,
			int createdBy,
			CancellationToken ct)
		{
			var (id, _) = await Insert(connection, projectId, name, configurationTypeId, createdBy, ct);

			return id;
		}

		internal async static Task<(int Id, int Noc)> Insert(
			SqlConnection connection,
			int projectId,
			string name,
			int configurationTypeId,
			int createdBy,
			CancellationToken ct)
		{
			const string ProcedureName = "ConfigurationRoot_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(ProjectIdParameter, projectId);
			parameters.Add(NameParameter, name);
			parameters.Add(ConfigurationTypeIdParameter, configurationTypeId);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var id = await connection.QuerySingleEx<int>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (id, noc);
		}

		public async static Task ConfigurationRootUpdate(
			this SqlConnection connection,
			string name,
			int? configurationTypeId,
			int modifiedBy,
			int id,
			CancellationToken ct)
		{
			_ = await Update(connection, name, configurationTypeId, modifiedBy, id, ct);
		}

		internal async static Task<int> Update(
			SqlConnection connection,
			string name,
			int? configurationTypeId,
			int modifiedBy,
			int id,
			CancellationToken ct)
		{
			const string ProcedureName = "ConfigurationRoot_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(NameParameter, name);
			parameters.Add(ConfigurationTypeIdParameter, configurationTypeId);
			parameters.AddModifiedBy(modifiedBy);
			parameters.AddId(id);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}

		public async static Task<dynamic> ConfigurationRootGetById(
			this SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			var (configuration, _) = await GetById(connection, id, ct);

			return configuration;
		}

		internal async static Task<(dynamic ConfigurationRoot, int Noc)> GetById(
			SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			const string Procedurename = "ConfigurationRoot_GetById";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddNoc();

			var root = await connection.QuerySingleOrDefaultEx<dynamic>(Procedurename, parameters, ct);
			var noc = parameters.GetNoc();

			return (root, noc);
		}

		public async static Task<IEnumerable<dynamic>> ConfigurationRootGetByProjectId(
			this SqlConnection connection,
			int projectId,
			CancellationToken ct)
		{
			var (configuratations, _) = await GetByProjectId(connection, projectId, ct);

			return configuratations;
		}

		internal async static Task<(IEnumerable<dynamic> ConfigurationRoots, int Noc)> GetByProjectId(
			this SqlConnection connection,
			int projectId,
			CancellationToken ct)
		{
			const string ProcedureName = "ConfigurationRoot_GetByProjectId";

			var parameters = new DynamicParameters();
			parameters.Add(ProjectIdParameter, projectId);
			parameters.AddNoc();

			var roots = await connection.QueryEx<dynamic>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (roots, noc);
		}

		public async static Task ConfigurationRootDelete(
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
			const string ProcedureName = "ConfigurationRoot_Delete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}
	}
}
