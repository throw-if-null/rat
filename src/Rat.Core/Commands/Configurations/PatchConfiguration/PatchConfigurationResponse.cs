using System;

namespace Rat.Core.Commands.Configurations.PatchConfiguration
{
	internal record PatchConfigurationResponse
	{
		public int ConfigurationId { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }

		public DateTimeOffset Created { get; init; }

		public DateTimeOffset Modified { get; init; }
	}
}
