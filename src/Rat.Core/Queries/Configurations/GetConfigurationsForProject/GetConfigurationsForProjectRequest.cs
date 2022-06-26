using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Core.Queries.Configurations.GetConfigurationsForProject
{
	internal record GetConfigurationsForProjectRequest : IRequest<GetConfigurationsForProjectResponse>
	{
		public int ProjectId { get; init; }
	}

	internal static class GetConfigurationForProjectRequestExtensions
	{
		public static void Validate(this GetConfigurationsForProjectRequest request)
		{
			var validationErrors = Validators.ValidateProjectId(request.ProjectId).ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
