namespace Rat.Data.Entities
{
	public class ConfigurationEntryEntity
	{
		public int Id { get; set; }

		public ConfigurationRootEntity Root { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public bool Disabled { get; set; }
	}
}
