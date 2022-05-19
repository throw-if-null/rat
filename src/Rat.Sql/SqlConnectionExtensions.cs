using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	internal static class SqlConnectionExtensions
	{
		public static Task<int> ExecuteEx(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters,
			CancellationToken ct) =>
			connection.ExecuteAsync(StoredProcedureDefinition.Create(name, parameters, ct));

		public static Task<T> QuerySingleEx<T>(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters,
			CancellationToken ct) =>
			connection.QuerySingleAsync<T>(StoredProcedureDefinition.Create(name, parameters, ct));

		public static Task<T> QuerySingleOrDefaultEx<T>(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters,
			CancellationToken ct) =>
			connection.QuerySingleOrDefaultAsync<T>(StoredProcedureDefinition.Create(name, parameters, ct));

		public static Task<IEnumerable<T>> QueryEx<T>(
			this SqlConnection connection,
			string name,
			DynamicParameters parameters,
			CancellationToken ct) =>
			connection.QueryAsync<T>(StoredProcedureDefinition.Create(name, parameters, ct));
	}
}
