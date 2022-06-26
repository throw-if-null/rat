using System.Collections.Generic;
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
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var project = await connection.ProjectGetById(request.ProjectId, cancellationToken);
			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.ProjectId} doesn't exist");

			var configuration = await connection.ConfigurationRootGetById(request.ConfigurationId, cancellationToken);
			if (configuration == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationId} does not exist");

			if (request.ProjectId != (int)configuration.ProjectId)
			{
				var kv = new KeyValuePair<string, string>(
					"ProjectId",
					$"Project: {request.ProjectId} does not match specified configuration: {request.ConfigurationId}");

				throw new InvalidRequestDataException(kv);
			}

			await connection.ConfigurationRootDelete(request.ConfigurationId, request.DeletedBy, cancellationToken);

			return new();
		}
	}
}
