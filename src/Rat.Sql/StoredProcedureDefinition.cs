using System.Data;
using Dapper;

namespace Rat.Sql
{
	public struct StoredProcedureDefinition
	{
		public CommandDefinition Command { get; }

		private StoredProcedureDefinition(string name, DynamicParameters parameters, CancellationToken ct)
		{
			Command = new CommandDefinition(name, parameters, commandType: CommandType.StoredProcedure, cancellationToken: ct);
		}

		public static CommandDefinition Create(string name, DynamicParameters parameters, CancellationToken ct)
			=> new StoredProcedureDefinition(name, parameters, ct).Command;
	}
}
