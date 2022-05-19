using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Sql;

namespace Rat.Commands.Projects.DeleteProject
{
	internal class DeleteProjectCommand : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

		public DeleteProjectCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<DeleteProjectResponse> Handle(
            [NotNull] DeleteProjectRequest request,
            CancellationToken cancellationToken)
        {
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var project = await connection.ProjectGetById(request.Id, cancellationToken);

			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.Id} does not exist");

			await connection.ProjectDelete(request.Id, 1, cancellationToken);

            return new();
        }
    }
}
