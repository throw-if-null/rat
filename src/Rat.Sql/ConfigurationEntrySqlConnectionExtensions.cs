using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	internal static class ConfigurationEntrySqlConnectionExtensions
	{
		private const string ConfigurationRootIdParameter = "@configurationRootId";
		private const string KeyParameter = "@key";
		private const string ValueParameter = "@value";
		private const string SecondsToLiveParameter = "@secondsToLive";
		private const string DisabledParameter = "@disabled";

		public async static Task<(int Id, int Noc)> ConfigurationEntryInsert(
			this SqlConnection connection,
			int configurationRootId,
			string key,
			string value,
			int secondToLive,
			bool disabled,
			int createdBy)
		{
			const string ProcedureName = "ConfigurationEntry_Insert";

			var parameters = new DynamicParameters();
			parameters.Add(ConfigurationRootIdParameter, configurationRootId);
			parameters.Add(KeyParameter, key);
			parameters.Add(ValueParameter, value);
			parameters.Add(SecondsToLiveParameter, secondToLive);
			parameters.Add(DisabledParameter, disabled);
			parameters.AddCreatedBy(createdBy);
			parameters.AddNoc();

			var id = await connection.QuerySingleEx<int>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			return (id, noc);
		}

		public async static Task<int> ConfigurationEntryUpdate(
			this SqlConnection connection,
				string key,
				string value,
				int? secondToLive,
				bool? disabled,
				int modifiedBy,
				int id)
		{
			const string ProcedureName = "ConfigurationEntry_Update";

			var parameters = new DynamicParameters();
			parameters.Add(KeyParameter, key);
			parameters.Add(ValueParameter, value);
			parameters.Add(SecondsToLiveParameter, secondToLive);
			parameters.Add(DisabledParameter, disabled);
			parameters.AddModifiedBy(modifiedBy);
			parameters.AddId(id);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}

		public async static Task<(dynamic ConfigurationEntry, int Noc)> ConfigurationEntryGetById(
			this SqlConnection connection,
			int id)
		{
			const string ProcedureName = "ConfigurationEntry_GetById";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddNoc();

			var entry = await connection.QuerySingleEx<dynamic>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			return (entry, noc);
		}

		public async static Task<(IEnumerable<dynamic> ConfigurationEntries, int Noc)> ConfigurationEntryGetByConfigurationRootId(
			this SqlConnection connection,
			int configurationRootId)
		{
			const string ProcedureName = "ConfigurationEntry_GetByConfigurationRootId";

			var parameters = new DynamicParameters();
			parameters.Add(ConfigurationRootIdParameter, configurationRootId);
			parameters.AddNoc();

			var entries = await connection.QueryEx<dynamic>(ProcedureName, parameters);
			var noc = parameters.GetNoc();

			return (entries, noc);
		}

		public async static Task<int> ConfigurationEntryDelete(
			this SqlConnection connection,
			int id,
			int deletedBy)
		{
			const string ProcedureName = "ConfigurationEntry_Delete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters);

			return noc;
		}
	}
}