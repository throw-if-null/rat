using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Sql
{
	public static class ProjectTypeSqlConnectionExtensions
    {
		public static async Task<(int Id, int NumberOfChanges)> ProjectTypeInsert(
			this SqlConnection connection,
			string name,
			int createdBy)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@name", name);
			parameters.Add("@createdBy", createdBy);
			parameters.Add("@numberOfChanges", dbType: DbType.Int32, direction: ParameterDirection.Output);

			var id = await connection.QuerySingleAsync<int>(
				"ProjectType_Insert",
				parameters,
				commandType: CommandType.StoredProcedure);

			var numberOfChanges = parameters.Get<int>("@numberOfChanges");

			return (id, numberOfChanges);
		}

		public static async Task<int> ProjectTypeUpdate(this SqlConnection connection,
			int id,
			string name,
			int modifiedBy)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@id", id);
			parameters.Add("@name", name);
			parameters.Add("@modifiedBy", modifiedBy);
			parameters.Add("@numberOfChanges", dbType: DbType.Int32, direction: ParameterDirection.Output);

			var numberOfChanges = await connection.ExecuteAsync(
				"ProjectType_Update",
				parameters,
				commandType: CommandType.StoredProcedure);

			return numberOfChanges;
		}
	}
}
