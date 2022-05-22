namespace Rat.Core.Commands.Configurations.CreateConfiguration
{
	internal record CreateConfigurationResponse
	{
		public int Id { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }
	}
}
