using System.Collections.Generic;

namespace Rat.Data.Entities
{
	public class ConfigurationRootEntity
	{
		public int Id { get; set; }

		public ConfigurationTypeEntity Type { get; set; }

		public string Name { get; set; }

		public ICollection<ConfigurationEntryEntity> Entries { get; set; } = new List<ConfigurationEntryEntity>();
	}
}
