namespace Rat.Sql
{
	internal class ConfigurationTypeTable : BaseTable
	{
		public override string TableName => nameof(ConfigurationTypeTable).Replace("Table", string.Empty);

		public const string Name = nameof(Name);
	}
}