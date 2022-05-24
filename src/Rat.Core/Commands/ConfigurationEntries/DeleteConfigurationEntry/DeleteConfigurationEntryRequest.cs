using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Commands.Validators;

namespace Rat.Core.Commands.ConfigurationEntries.DeleteConfigurationEntry
{
	internal class DeleteConfigurationEntryRequest : IRequest<DeleteConfigurationEntryResponse>
	{
		public int Id { get; init; }

		public int DeletedBy { get; init; }
	}

	internal static class DeleteConfigurationEntryRequestExtensions
	{
		public static void Validate(this DeleteConfigurationEntryRequest request)
		{
			var validationErrors =
				ValidateId(request.Id)
				.Union(ValidateDeletedBy(request.DeletedBy))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
