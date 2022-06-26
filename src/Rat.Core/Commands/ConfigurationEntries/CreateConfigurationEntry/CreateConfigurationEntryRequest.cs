using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Core.Commands.ConfigurationEntries.CreateConfigurationEntry
{
	internal class CreateConfigurationEntryRequest : IRequest<CreateConfigurationEntryResponse>
	{
		public int ConfigurationRootId { get; init; }

		public string Key { get; init; }

		public string Value { get; init; }

		public int SecondsToLive { get; init; } = -1;

		public bool Disabled { get; init; } = false;

		public int CraetedBy { get; init; }
	}

	internal static class CreateConfigurationEntryRequestExtensions
	{
		public static void Validate(this CreateConfigurationEntryRequest request)
		{
			var validationErrors =
				Validators.ValidateKey(request.Key)
				.Union(Validators.ValidateValue(request.Value))
				.Union(Validators.ValidateConfigurationRootId(request.ConfigurationRootId))
				.Union(Validators.ValidateSecondsToLive(request.SecondsToLive))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
