using System.Data;
using Dapper;

namespace Rat.Sql
{
	public struct StoredProcedureDefinition
	{
		public CommandDefinition Command { get; }

		private StoredProcedureDefinition(string name, DynamicParameters parameters)
		{
			Command = new CommandDefinition(name, parameters, commandType: CommandType.StoredProcedure);
		}

		public static CommandDefinition Create(string name, DynamicParameters parameters)
			=> new StoredProcedureDefinition(name, parameters).Command;
	}
}
