namespace Rat.DataAccess.Entities
{
	public class ConfigurationEntryEntity
	{
		public int Id { get; set; }

		public int ConfigurationRootId { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public bool Disabled { get; set; }
	}
}
