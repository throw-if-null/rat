using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Core.Commands.Configurations.PatchConfiguration
{
	internal record PatchConfigurationRequest : IRequest<PatchConfigurationResponse>
	{
		public int ProjectId { get; init; }

		public int Id { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }

		public int ModifiedBy { get; init; }
	}

	internal static class PatchConfigurationRequestExtensions
	{
		public static void Validate(this PatchConfigurationRequest request)
		{
			var validationErrors =
				ValidateId(request.Id)
				.Union(Validators.ValidateProjectId(request.ProjectId))
				.Union(Validators.ValidateName(request.Name))
				.Union(Validators.ValidateConfigurationTypeId(request.ConfigurationTypeId))
				.Union(ValidateModifiedBy(request.ModifiedBy))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
