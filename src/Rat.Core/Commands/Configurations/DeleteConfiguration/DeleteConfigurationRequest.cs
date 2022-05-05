using MediatR;

namespace Rat.Core.Commands.Configurations.DeleteConfiguration
{
	internal record DeleteConfigurationRequest : IRequest<DeleteConfigurationResponse>
	{
		public int ConfigurationId { get; init; }
	}
}
