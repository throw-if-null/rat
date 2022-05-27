using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Core.Queries.Configurations.GetConfiguration
{
	internal class GetConfigurationQuery : IRequestHandler<GetConfigurationRequest, GetConfigurationResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public GetConfigurationQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetConfigurationResponse> Handle(GetConfigurationRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();
			var configuration = await connection.ConfigurationRootGetById(request.ConfigurationId, cancellationToken);

			if (configuration == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationId} not found");

			var entries = await connection.ConfigurationEntryGetByConfigurationRootId(request.ConfigurationId, cancellationToken);

			return new()
			{
				ConfigurationId = configuration.Id,
				Name = configuration.Name,
				ConfigurationTypeId = configuration.ConfigurationTypeId,
				ConfigurationEntries = entries.Select(x => new ConfigurationEntry
				{
					Id = x.Id,
					Key = x.Key,
					Value = x.Value,
					Disabled = x.Disabled,
					SecondsToLive = x.SecondsToLive
				})
			};
		}
	}
}
