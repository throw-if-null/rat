using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Core.Commands.Configurations.PatchConfiguration
{
	internal class PatchConfigurationCommand : IRequestHandler<PatchConfigurationRequest, PatchConfigurationResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public PatchConfigurationCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<PatchConfigurationResponse> Handle(PatchConfigurationRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var configuration = await connection.ConfigurationRootGetById(request.Id, cancellationToken);
			if (configuration == null)
				throw new ResourceNotFoundException($"Configuration: {request.Id} does not exist");

			// TODO: Check if ConfigurationType exists

			await connection.ConfigurationRootUpdate(
				request.Name == configuration.Name ? null : request.Name,
				request.ConfigurationTypeId == configuration.ConfigurationTypeId ? null : request.ConfigurationTypeId,
				request.ModifiedBy,
				request.Id,
				cancellationToken);

			configuration = await connection.ConfigurationRootGetById(request.Id, CancellationToken.None);

			return new()
			{
				ConfigurationId = configuration.Id,
				ConfigurationTypeId = configuration.ConfigurationTypeId,
				Name = configuration.Name
			};
		}
	}
}
