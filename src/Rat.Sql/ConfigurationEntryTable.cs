namespace Rat.Sql
{
	internal class ConfigurationEntryTable : BaseTable
	{
		public override string TableName => nameof(ConfigurationEntryTable).Replace("Table", string.Empty);

		public const string Name = nameof(Name);
		public const string Key = nameof(Key);
		public const string Value = nameof(Value);
		public const string Disabled = nameof(Disabled);
		public const string SecondsToLive = nameof(SecondsToLive);
	}
}
