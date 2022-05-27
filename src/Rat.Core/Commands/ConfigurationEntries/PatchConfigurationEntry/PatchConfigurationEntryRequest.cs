using System.Linq;
using MediatR;
using Rat.Core.Commands.ConfigurationEntries.CreateConfigurationEntry;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Core.Commands.ConfigurationEntries.PatchConfigurationEntry
{
	internal class PatchConfigurationEntryRequest : IRequest<PatchConfigurationEntryResponse>
	{
		public int Id { get; init; }

		public string Key { get; init; }

		public string Value { get; init; }

		public int? SecondsToLive { get; init; }

		public bool? Disabled { get; init; }

		public int ConfigurationRootId { get; init; }

		public int ModifiedBy { get; init; }
	}

	internal static class PatchConfigurationEntryRequestExtensions
	{
		public static void Validate(this PatchConfigurationEntryRequest request)
		{
			var validationErrors =
				ValidateId(request.Id)
				.Union(Validators.ValidateConfigurationRootId(request.ConfigurationRootId))
				.Union(ValidateModifiedBy(request.ModifiedBy))
				.Union(Validators.AllNull(request)).ToList();

			if (!string.IsNullOrWhiteSpace(request.Key))
				validationErrors.AddRange(Validators.ValidateKeyMaxLenghth(request.Key));

			if (!string.IsNullOrWhiteSpace(request.Value))
				validationErrors.AddRange(Validators.ValidateValueMaxLenght(request.Value));

			if (request.SecondsToLive.HasValue)
				validationErrors.AddRange(Validators.ValidateSecondsToLive(request.SecondsToLive.Value));

			if (validationErrors.Count == 0)
				return;

			throw new InvalidRequestDataException(validationErrors.ToArray());
		}
	}
}
