namespace Rat.Core.Commands.Configurations.CreateConfiguration
{
	internal record CreateConfigurationResponse
	{
		public object Id { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }
	}
}
