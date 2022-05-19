using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Core.Queries.Configurations.GetConfigurationsForProject
{
	internal class GetConfigurationsForProjectQuery : IRequestHandler<GetConfigurationsForProjectRequest, GetConfigurationsForProjectResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public GetConfigurationsForProjectQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetConfigurationsForProjectResponse> Handle(GetConfigurationsForProjectRequest request, CancellationToken cancellationToken)
		{
			using var connection = _connectionFactory.CreateConnection();

			var project = await connection.ProjectGetById(request.ProjectId, cancellationToken);
			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.ProjectId} does not exist");

			var configurations = await connection.ConfigurationRootGetByProjectId(request.ProjectId, cancellationToken);
			configurations ??= Array.Empty<dynamic>();

			return new GetConfigurationsForProjectResponse
			{
				Configurations = configurations.Select(x => new GetConfigurationForProjectResponse
				{
					Id = x.Id,
					Name = x.Name,
					ConfigurationTypeId = x.ConfigurationTypeId,
					Entries = x.ConfigurationEntryCount
				}).ToArray()
			};
		}
	}
}
