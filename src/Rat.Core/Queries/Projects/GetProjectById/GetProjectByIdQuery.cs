using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Queries.Projects.GetProjectById
{
	internal class GetProjectByIdQuery : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public GetProjectByIdQuery(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<GetProjectByIdResponse> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();
			var project = await connection.ProjectGetById(request.Id, cancellationToken);

			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.Id} doesn't exist");

			int projectId = project.Id;

			var configurations = await connection.ConfigurationRootGetByProjectId(projectId, cancellationToken);

			return new()
			{
				Id = project.Id,
				Name = project.Name,
				TypeId = project.ProjectTypeId,
				Configurations = configurations.Select(x => new ConfigurationInfo(x.Id, x.Name, x.ConfigurationTypeId, x.ConfigurationEntryCount))
			};
		}
	}
}
