using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Core.Commands.Configurations.PatchConfiguration
{
	internal record PatchConfigurationRequest : IRequest<PatchConfigurationResponse>
	{
		public int ConfigurationId { get; init; }

		public string Name { get; init; }

		public int ConfigurationTypeId { get; init; }

		public int ModifiedBy { get; init; }
	}

	internal static class PatchConfigurationRequestExtensions
	{
		public static void Validate(this PatchConfigurationRequest request)
		{
			var validationErrors =
				ValidateId(request.ConfigurationId)
				.Union(ValidateModifiedBy(request.ModifiedBy))
				.Union(Validators.ValidateName(request.Name))
				.Union(Validators.ValidateConfigurationTypeId(request.ConfigurationTypeId))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
