using System;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Rat.Api.Test
{
	internal static class TestSqlConnectionExtensions
	{
		public static bool DatabaseExists(this SqlConnection connection, string name)
		{
			var command = new CommandDefinition(
				"SELECT name FROM sys.databases WHERE name = @DatabaseName",
				new { DatabaseName = name });

			connection.ChangeDatabase("master");
			var result = connection.QueryFirstOrDefault<string>(command);

			return !string.IsNullOrWhiteSpace(result);
		}

		public static void DropDatabase(this SqlConnection connection, string name)
		{
			var query =
				$@"ALTER DATABASE {name}
				SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
				DROP DATABASE {name}";

			connection.ChangeDatabase("master");
			connection.Execute(new CommandDefinition(query));
		}

		public static void CreateDatabase(this SqlConnection connection, string name)
		{
			var query = $@"CREATE DATABASE {name}";

			connection.ChangeDatabase("master");
			connection.Execute(new CommandDefinition(query));
		}

		public static void CreateTable(this SqlConnection connection, string database, string name)
		{
			var path = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");

			var query = File.ReadAllText(Path.Combine(path, "src/Rat.Database/Tables", name));

			connection.ChangeDatabase(database);
			connection.Execute(new CommandDefinition(query));
		}

		public static Task<int> ProjectTypeGetByName(this SqlConnection connection, string name)
		{
			var command = new CommandDefinition(
				"SELECT Id FROM ProjectType WHERE Name = @name",
				new { name = name });

			return connection.QuerySingleAsync<int>(command);
		}

		public static Task<int> ConfigurationTypeGetByName(this SqlConnection connection, string name)
		{
			var command = new CommandDefinition(
				"SELECT Id FROM ConfigurationType WHERE Name = @name",
				new { name = name });

			return connection.QuerySingleAsync<int>(command);
		}
	}
}
