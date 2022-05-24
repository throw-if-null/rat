using MediatR;

namespace Rat.Core.Queries.Configurations.GetConfiguration
{
	internal record GetConfigurationRequest : IRequest<GetConfigurationResponse>
	{
		public int ConfigurationId { get; init; }

		public bool IncludeEntries { get; init; } = true;
	}
}
