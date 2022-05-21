using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

			await connection.ConfigurationEntryUpdate(
				request.Key,
				request.Value,
				request.SecondsToLive,
				request.Disabled,
				request.ModifiedBy,
				request.Id,
				cancellationToken);

			return new();
		}
	}
}
