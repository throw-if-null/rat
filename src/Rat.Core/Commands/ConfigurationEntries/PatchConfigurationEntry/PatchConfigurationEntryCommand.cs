using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Core.Commands.ConfigurationEntries.PatchConfigurationEntry
{
	internal class PatchConfigurationEntryCommand : IRequestHandler<PatchConfigurationEntryRequest, PatchConfigurationEntryResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public PatchConfigurationEntryCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<PatchConfigurationEntryResponse> Handle(PatchConfigurationEntryRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var configuration = await connection.ConfigurationRootGetById(request.ConfigurationRootId, cancellationToken);

			if (configuration == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationRootId} doesn't exist");

			var entry = await connection.ConfigurationEntryGetById(request.Id, cancellationToken);

			if (entry == null)
				throw new ResourceNotFoundException($"ConfigurationEntry:{request.Id} doesn't exist");

			await connection.ConfigurationEntryUpdate(
				request.Key,
				request.Value,
				request.SecondsToLive,
				request.Disabled,
				request.ModifiedBy,
				request.Id,
				cancellationToken);

			entry = await connection.ConfigurationEntryGetById(request.Id, cancellationToken);

			return new()
			{
				Id = (int)entry.Id,
				Key = (string)entry.Key,
				Value = (string)entry.Value,
				SecondsToLive = (int)entry.SecondsToLive,
				Disabled = (bool) entry.Disabled,
				ModifiedBy = (int)entry.Operator,
				ModifiedOn = (DateTimeOffset)entry.Timestamp,
				ConfigurationRootId = (int)entry.ConfigurationRootId
			};
		}
	}
}
