using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	internal static class SqlConnectionExtensions
	{
		public static Task<int> ExecuteEx(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters) =>
			connection.ExecuteAsync(StoredProcedureDefinition.Create(name, parameters));

		public static Task<T> QuerySingleEx<T>(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters) =>
			connection.QuerySingleAsync<T>(StoredProcedureDefinition.Create(name, parameters));

		public static Task<T> QuerySingleOrDefaultEx<T>(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters) =>
			connection.QuerySingleOrDefaultAsync<T>(StoredProcedureDefinition.Create(name, parameters));

		public static Task<IEnumerable<T>> QueryEx<T>(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters) =>
			connection.QueryAsync<T>(StoredProcedureDefinition.Create(name, parameters));
	}
}
