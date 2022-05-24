using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class ConfigurationEntrySqlConnectionExtensions
	{
		private const string ConfigurationRootIdParameter = "@configurationRootId";
		private const string KeyParameter = "@key";
		private const string ValueParameter = "@value";
		private const string SecondsToLiveParameter = "@secondsToLive";
		private const string DisabledParameter = "@disabled";

		public async static Task<int> ConfigurationEntryInsert(
			this SqlConnection connection,
			int configurationRootId,
			string key,
			string value,
			int secondToLive,
			bool disabled,
			int createdBy,
			CancellationToken ct)
		{
			var (id, _) = await Insert(connection, configurationRootId, key, value, secondToLive, disabled, createdBy, ct);

			return id;
		}

		internal async static Task<(int Id, int Noc)> Insert(
			SqlConnection connection,
			int configurationRootId,
			string key,
			string value,
			int secondToLive,
			bool disabled,
			int createdBy,
			CancellationToken ct)
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

			var id = await connection.QuerySingleEx<int>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (id, noc);
		}

		public async static Task ConfigurationEntryUpdate(
			this SqlConnection connection,
			string key,
			string value,
			int? secondToLive,
			bool? disabled,
			int modifiedBy,
			int id,
			CancellationToken ct)
		{
			_ = await Update(connection, key, value, secondToLive, disabled, modifiedBy, id, ct);
		}

		internal async static Task<int> Update(
			SqlConnection connection,
			string key,
			string value,
			int? secondToLive,
			bool? disabled,
			int modifiedBy,
			int id,
			CancellationToken ct)
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

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}

		public async static Task<dynamic> ConfigurationEntryGetById(
			this SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			var (entry, _) = await GetById(connection, id, ct);

			return entry;
		}
		internal async static Task<(dynamic ConfigurationEntry, int Noc)> GetById(
			SqlConnection connection,
			int id,
			CancellationToken ct)
		{
			const string ProcedureName = "ConfigurationEntry_GetById";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddNoc();

			var entry = await connection.QuerySingleEx<dynamic>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (entry, noc);
		}

		public async static Task<IEnumerable<dynamic>> ConfigurationEntryGetByConfigurationRootId(
			this SqlConnection connection,
			int configurationRootId,
			CancellationToken ct)
		{
			var (entries, _) = await GetByConfigurationRootId(connection, configurationRootId, ct);

			return entries;
		}

		internal async static Task<(IEnumerable<dynamic> ConfigurationEntries, int Noc)> GetByConfigurationRootId(
			SqlConnection connection,
			int configurationRootId,
			CancellationToken ct)
		{
			const string ProcedureName = "ConfigurationEntry_GetByConfigurationRootId";

			var parameters = new DynamicParameters();
			parameters.Add(ConfigurationRootIdParameter, configurationRootId);
			parameters.AddNoc();

			var entries = await connection.QueryEx<dynamic>(ProcedureName, parameters, ct);
			var noc = parameters.GetNoc();

			return (entries, noc);
		}

		public async static Task ConfigurationEntryDelete(
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
			const string ProcedureName = "ConfigurationEntry_Delete";

			var parameters = new DynamicParameters();
			parameters.AddId(id);
			parameters.AddDeletedBy(deletedBy);
			parameters.AddNoc();

			var noc = await connection.ExecuteEx(ProcedureName, parameters, ct);

			return noc;
		}
	}
}