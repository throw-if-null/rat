﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Sql;

namespace Rat.Core.Commands.Configurations.CreateConfiguration
{
	internal class CreateConfigurationCommand
		: IRequestHandler<CreateConfigurationRequest, CreateConfigurationResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public CreateConfigurationCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<CreateConfigurationResponse> Handle(CreateConfigurationRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var id = await connection.ConfigurationRootInsert(
				request.Name, 
				request.ConfigurationTypeId, 
				request.CreatedBy, 
				cancellationToken);
			var configuration = await connection.ConfigurationRootGetById(id, cancellationToken);

			return new()
			{
				Id = id,
				Name = configuration.Name,
				ConfigurationTypeId = configuration.ConfigurationTypeId
			};
		}
	}
}
