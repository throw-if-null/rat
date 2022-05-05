namespace Rat.Sql
{
	internal class ProjectTypeTable : BaseTable
	{
		public override string TableName => nameof(ProjectTypeTable).Replace("Table", string.Empty);

		public const string Name = nameof(Name);

		public string Insert() => InsertQueryBuilder.Build(TableName, new[] { Name });

		public string GetById() => SelectQueryBuilder.BuildGetById(TableName, new [] { Id, Name, Created, Modified});

		public string GetAll() => SelectQueryBuilder.BuildGetAll(TableName, new[] { Id, Name, Created, Modified });

		public string UpdateName() => UpdateQueryBuilder.Build(TableName, new string[] { Name });

		public string Delete() => DeleteQueryBuilder.Build(TableName);
	}

	internal static class InsertQueryBuilder
	{
		public static string Build(string tableName, string[] properties)
		{
			var names = string.Empty;
			var values = string.Empty;

			foreach (var property in properties)
			{
				names += $"{property},";
				values += $"@{property},";
			}

			names = names.Remove(names.Length -1, 1);
			values = values.Remove(values.Length - 1, 1);

			var query = $"INSERT INTO {tableName} ({names}) VALUES({values})";

			return query;
		}
	}

	internal static class SelectQueryBuilder
	{
		public static string BuildGetById(string tableName, string[] columns)
		{
			var query = $"SELECT {columns} FROM {tableName} WHERE Id = @Id AND DELETE IS NULL";

			return query;
		}

		public static string BuildGetAll(string tableName, string[] columns)
		{
			var query = $"SELECT {columns} FROM {tableName} WHERE DELETED IS NULL";

			return query;
		}
	}

	internal static class UpdateQueryBuilder
	{
		public static string Build(string tableName, string[] columns)
		{
			var update = string.Empty;

			foreach(var column in columns)
			{
				update += $"{column} = @{column}";
			}

			var query = $"UPDATE {tableName} SET {update}, Modified = GETUTCDATE() WHERE Id = @Id";

			return query;
		}
	}

	internal static class DeleteQueryBuilder
	{
		public static string Build(string tableName)
		{
			var query = $"DELECT FROM {tableName} WHERE Id = @Id";

			return query;
		}
	}
}
