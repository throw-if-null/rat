namespace Rat.Sql
{
	internal class ConfigurationRootTable : BaseTable
	{
		public override string TableName => nameof(ConfigurationRootTable).Replace("Table", string.Empty);

		public const string Name = nameof(Name);
		public const string ConfigurationTypeId = nameof(ConfigurationTypeId);
	}
}
