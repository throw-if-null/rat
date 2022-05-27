using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Core.Queries.Configurations.GetConfiguration
{
	internal record GetConfigurationRequest : IRequest<GetConfigurationResponse>
	{
		public int ConfigurationId { get; init; }
	}

	internal static class GetConfigurationRequestExtensions
	{
		public static void Validate(this GetConfigurationRequest request)
		{
			var validationErrors = ValidateId(request.ConfigurationId).ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
