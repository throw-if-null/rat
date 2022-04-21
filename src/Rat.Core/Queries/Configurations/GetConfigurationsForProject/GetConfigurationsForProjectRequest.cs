using MediatR;

namespace Rat.Core.Queries.Configurations.GetConfigurationsForProject
{
	internal record GetConfigurationsForProjectRequest : IRequest<GetConfigurationsForProjectResponse>
	{
		public int ProjectId { get; init; }
	}
}
