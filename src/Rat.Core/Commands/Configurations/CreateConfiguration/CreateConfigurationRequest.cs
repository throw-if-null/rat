using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Core.Commands.Configurations.CreateConfiguration
{
	internal record CreateConfigurationRequest : IRequest<CreateConfigurationResponse>
	{
		public int ConfigurationTypeId { get; init; }

		public string Name { get; init; }
	}

	internal static class CreateConfigurationRequestExtensions
	{
		public static void Validate(this CreateConfigurationRequest request)
		{
			var validationErrors =
				Validators.ValidateName(request.Name)
				.Union(Validators.ValidateConfigurationTypeId(request.ConfigurationTypeId))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
