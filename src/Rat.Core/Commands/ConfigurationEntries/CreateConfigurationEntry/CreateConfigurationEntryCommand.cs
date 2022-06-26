using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Sql;

namespace Rat.Core.Commands.ConfigurationEntries.CreateConfigurationEntry
{
	internal class CreateConfigurationEntryCommand : IRequestHandler<CreateConfigurationEntryRequest, CreateConfigurationEntryResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public CreateConfigurationEntryCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<CreateConfigurationEntryResponse> Handle(CreateConfigurationEntryRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var id = await connection.ConfigurationEntryInsert(
				request.ConfigurationRootId,
				request.Key,
				request.Value,
				request.SecondsToLive,
				request.Disabled,
				request.CraetedBy,
				cancellationToken);

			return new() { Id = id };
		}
	}
}
