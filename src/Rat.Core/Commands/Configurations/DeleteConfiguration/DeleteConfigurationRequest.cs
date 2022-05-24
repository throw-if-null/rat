using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Commands.Validators;

namespace Rat.Core.Commands.Configurations.DeleteConfiguration
{
	internal record DeleteConfigurationRequest : IRequest<DeleteConfigurationResponse>
	{
		public int ConfigurationId { get; init; }

		public int DeletedBy { get; init; }
	}

	internal static class DeleteConfigurationRequestExtensions
	{
		public static void Validate(this DeleteConfigurationRequest request)
		{
			var validationErrors =
				ValidateId(request.ConfigurationId)
				.Union(ValidateDeletedBy(request.DeletedBy))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
