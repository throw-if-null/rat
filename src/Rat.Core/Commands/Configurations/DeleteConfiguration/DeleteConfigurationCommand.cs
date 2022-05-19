using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Core.Commands.Configurations.DeleteConfiguration
{
	internal class DeleteConfigurationCommand : IRequestHandler<DeleteConfigurationRequest, DeleteConfigurationResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public DeleteConfigurationCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<DeleteConfigurationResponse> Handle(DeleteConfigurationRequest request, CancellationToken cancellationToken)
		{
			await using var connection = _connectionFactory.CreateConnection();

			var configuration = await connection.ConfigurationRootGetById(request.ConfigurationId, cancellationToken);
			if (configuration == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationId} does not exist");

			await connection.ConfigurationRootDelete(request.ConfigurationId, 1, cancellationToken);

			return new();
		}
	}
}
