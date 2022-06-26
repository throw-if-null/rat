using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Core.Commands.Configurations.DeleteConfiguration
{
	internal record DeleteConfigurationRequest : IRequest<DeleteConfigurationResponse>
	{
		public int ConfigurationId { get; init; }

		public int ProjectId { get; init; }

		public int DeletedBy { get; init; }
	}

	internal static class DeleteConfigurationRequestExtensions
	{
		public static void Validate(this DeleteConfigurationRequest request)
		{
			var validationErrors =
				ValidateId(request.ConfigurationId)
				.Union(Validators.ValidateProjectId(request.ProjectId))
				.Union(ValidateDeletedBy(request.DeletedBy))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
