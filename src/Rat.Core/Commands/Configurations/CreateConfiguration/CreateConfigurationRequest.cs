using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Commands.Validators;

namespace Rat.Core.Commands.Configurations.CreateConfiguration
{
	internal record CreateConfigurationRequest : IRequest<CreateConfigurationResponse>
	{
		public int ProjectId { get; init; }

		public int ConfigurationTypeId { get; init; }

		public string Name { get; init; }

		public int CreatedBy { get; init; }
	}

	internal static class CreateConfigurationRequestExtensions
	{
		public static void Validate(this CreateConfigurationRequest request)
		{
			var validationErrors =
				Validators.ValidateName(request.Name)
				.Union(Validators.ValidateProjectId(request.ProjectId))
				.Union(Validators.ValidateConfigurationTypeId(request.ConfigurationTypeId))
				.Union(ValidateCreatedBy(request.CreatedBy))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
