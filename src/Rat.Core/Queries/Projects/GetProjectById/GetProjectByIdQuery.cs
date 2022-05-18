using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.DataAccess;
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
			var project = await connection.ProjectGetById(request.Id);

			return project == null
				? null
				: new()
				{
					Id = project.Id,
					Name = project.Name,
					TypeId = project.ProjectTypeId
				};
		}
	}
}
