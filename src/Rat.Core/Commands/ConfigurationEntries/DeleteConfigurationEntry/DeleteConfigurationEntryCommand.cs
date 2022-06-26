using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Sql;

namespace Rat.Core.Commands.ConfigurationEntries.DeleteConfigurationEntry
{
	internal class DeleteConfigurationEntryCommand : IRequestHandler<DeleteConfigurationEntryRequest, DeleteConfigurationEntryResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public DeleteConfigurationEntryCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<DeleteConfigurationEntryResponse> Handle(DeleteConfigurationEntryRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			await connection.ConfigurationEntryDelete(request.Id, request.DeletedBy, cancellationToken);

			return new();
		}
	}
}
