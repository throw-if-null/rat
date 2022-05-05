using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Core.Properties;
using Rat.Core.Queries.Configurations.GetConfiguration;
using Rat.DataAccess;

namespace Rat.Core.Commands.Configurations.PatchConfiguration
{
	internal class PatchConfigurationCommand : IRequestHandler<PatchConfigurationRequest, PatchConfigurationResponse>
	{
		private const string Query =
			@"UPDATE ConfigurationRoot
			SET Name = @Name, ConfigurationTypeId = @ConfigurationTypeId, Modified = GETUTCDATE()
			WHERE Id = @Id";

		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public PatchConfigurationCommand(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
		}

		public async Task<PatchConfigurationResponse> Handle(PatchConfigurationRequest request, CancellationToken cancellationToken)
		{
			var getConfigurationResponse = await _mediator.Send(new GetConfigurationRequest { ConfigurationId = request.ConfigurationId, IncludeEntries = false });

			if (getConfigurationResponse == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationId} does not exist");

			// TODO: Check if ConfigurationTypeId exists

			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(
				Query,
				new { Id = request.ConfigurationId, Name = request.Name, ConfigurationTypeId = request.ConfigurationTypeId },
				cancellationToken: cancellationToken);

			var changes = await connection.ExecuteAsync(command);
			var expectedNumberOfChanges = 1;

			if (changes != expectedNumberOfChanges)
				throw new RatDbException(string.Format(Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch, changes, expectedNumberOfChanges));

			getConfigurationResponse = await _mediator.Send(new GetConfigurationRequest { ConfigurationId = request.ConfigurationId });

			return new()
			{
				ConfigurationId = getConfigurationResponse.ConfigurationId,
				ConfigurationTypeId = getConfigurationResponse.ConfigurationTypeId,
				Name = getConfigurationResponse.Name,
				Created = getConfigurationResponse.Created,
				Modified = getConfigurationResponse.Modified
			};
		}
	}
}
