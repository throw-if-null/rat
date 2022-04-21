using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.DataAccess;

namespace Rat.Core.Commands.Configurations.CreateConfiguration
{
	internal class CreateConfigurationCommand
		: IRequestHandler<CreateConfigurationRequest, CreateConfigurationResponse>
	{
		private const string Query =
			@"INSERT INTO ConfigurationRoot (Name, ConfigurationTypeId) VALUES(@Name, @ConfigurationTypeId)
            DECLARE @Id INT = (SELECT SCOPE_IDENTITY())
			SELECT @Id";

		private readonly ISqlConnectionFactory _connectionFactory;

		public CreateConfigurationCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<CreateConfigurationResponse> Handle(CreateConfigurationRequest request, CancellationToken cancellationToken)
		{
			await using var connection = _connectionFactory.CreateConnection();
			var command = new CommandDefinition(
				Query,
				new { Name = request.Name, ConfigurationTypeId = request.ConfigurationTypeId },
				cancellationToken: cancellationToken);

			var id = await connection.QuerySingleAsync<int>(command);

			return new()
			{
				Id = id,
				Name = request.Name,
				ConfigurationTypeId = request.ConfigurationTypeId
			};
		}
	}
}
