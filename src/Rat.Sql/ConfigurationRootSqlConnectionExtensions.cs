using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	internal static class ConfigurationRootSqlConnectionExtensions
	{
		private const string NameParameter = "@name";
		private const string ConfigurationTypeIdParameter = "@configurationTypeId";
		private const string ProjectIdParameter = "@projectId";

		public async static Task<(int Id, int Noc)> ConfigurationRootInsert(
			this SqlConnection connection,
			string name,
			int configurationTypeId,
			int createdBy)
		{
			const string ProcedureName = "ConfigurationRoot_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(NameParameter, name);
			parameters.Add(ConfigurationTypeIdParameter, configurationTypeId);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var id = await connection.QuerySingleEx<int>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			return (id, noc);
		}

		public async static Task<int> ConfigurationRootUpdate(
			this SqlConnection connection,
			string name,
			int? configurationTypeId,
			int modifiedBy,
			int id)
		{
			const string ProcedureName = "ConfigurationRoot_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(NameParameter, name);
			parameters.Add(ConfigurationTypeIdParameter, configurationTypeId);
			parameters.AddModifiedBy(modifiedBy);
			parameters.AddId(id);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}

		public async static Task<(dynamic ConfigurationRoot, int Noc)> ConfigurationRootGetById(
			this SqlConnection connection,
			int id)
		{
			const string Procedurename = "ConfigurationRoot_GetById";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddNoc();

			var root = await connection.QuerySingleOrDefaultEx<dynamic>(Procedurename, parameters);
			var noc = parameters.GetNoc();

			return (root, noc);
		}

		public async static Task<(IEnumerable<dynamic> ConfigurationRoots, int Noc)> ConfigurationRootGetByProjectId(
			this SqlConnection connection,
			int projectId)
		{
			const string ProcedureName = "ConfigurationRoot_GetByProjectId";

			var parameters = new DynamicParameters();
			parameters.Add(ProjectIdParameter, projectId);
			parameters.AddNoc();

			var roots = await connection.QueryEx<dynamic>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			return (roots, noc);
		}

		public async static Task<int> ConfigurationRootDelete(
			this SqlConnection connection,
			int id,
			int deletedBy)
		{
			const string ProcedureName = "ConfigurationRoot_Delete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}
	}
}
