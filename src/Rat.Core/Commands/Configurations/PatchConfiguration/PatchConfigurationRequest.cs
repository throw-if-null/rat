using MediatR;

namespace Rat.Core.Commands.Configurations.PatchConfiguration
{
	internal record PatchConfigurationRequest : IRequest<PatchConfigurationResponse>
	{
		public int ConfigurationId { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }
	}
}
