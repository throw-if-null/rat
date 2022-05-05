using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.DataAccess;

namespace Rat.Core.Queries.Configurations.GetConfiguration
{
	internal class GetConfigurationQuery : IRequestHandler<GetConfigurationRequest, GetConfigurationResponse>
	{
		private const string Query = "SELECT Id, Name, ConfigurationTypeId, Created, Modified FROM ConfigurationRoot WHERE Id = @Id AND Deleted IS NULL";
		private const string Query2 = "SELECT Id, Key, Value, SecondsToLive, Disabled, Created, Modified FROM ConfigurationEntry WHERE ConfigurationRootId = @ConfigurationRootId AND Deleted IS NULL";
		private readonly ISqlConnectionFactory _connectionFactory;

		public GetConfigurationQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetConfigurationResponse> Handle(GetConfigurationRequest request, CancellationToken cancellationToken)
		{
			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(Query, new { Id = request.ConfigurationId }, cancellationToken: cancellationToken);

			var configuration = await connection.QuerySingleOrDefaultAsync(command);

			if (configuration == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationId} not found");

			command = new CommandDefinition(
				Query2,
				new { ConfigurationRootId = request.ConfigurationId },
				cancellationToken: cancellationToken);

			var configurationEntries = (await connection.QueryAsync(command)) ?? Enumerable.Empty<dynamic>();

			return new()
			{
				ConfigurationId = configuration.Id,
				Name = configuration.Name,
				ConfigurationTypeId = configuration.ConfigurationTypeId,
				Created = configuration.Created,
				Modified = configuration.Modified,
				ConfigurationEntries = configurationEntries.Select(x => new ConfigurationEntry
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
