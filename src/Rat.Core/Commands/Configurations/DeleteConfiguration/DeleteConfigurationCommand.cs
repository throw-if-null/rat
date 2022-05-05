using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Core.Properties;
using Rat.Core.Queries.Configurations.GetConfiguration;
using Rat.DataAccess;

namespace Rat.Core.Commands.Configurations.DeleteConfiguration
{
	internal class DeleteConfigurationCommand : IRequestHandler<DeleteConfigurationRequest, DeleteConfigurationResponse>
	{
		private const string Query = "UPDATE ConfigurationRoots SET Delete = GETUTCDATE() WHERE Id = @Id";

		private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public DeleteConfigurationCommand(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
		}

		public async Task<DeleteConfigurationResponse> Handle(DeleteConfigurationRequest request, CancellationToken cancellationToken)
		{
			var getConfigurationResponse = await _mediator.Send(new GetConfigurationRequest { ConfigurationId = request.ConfigurationId, IncludeEntries = false });

			if (getConfigurationResponse == null)
				throw new ResourceNotFoundException($"Configuration: {request.ConfigurationId} does not exist");

			await using var connection = _connectionFactory.CreateConnection();
			var command = new CommandDefinition(
				Query,
				new { Id = request.ConfigurationId },
				cancellationToken: cancellationToken);

			var changes = await connection.ExecuteAsync(command);
			var expectedNumberOfChanges = 1;

			if (changes != expectedNumberOfChanges)
				throw new RatDbException(string.Format(Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch, changes, expectedNumberOfChanges));

			return new();
		}
	}
}
