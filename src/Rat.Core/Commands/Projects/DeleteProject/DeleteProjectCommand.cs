using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.DataAccess;
using Rat.Sql;

namespace Rat.Commands.Projects.DeleteProject
{
	internal class DeleteProjectCommand : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
    {
		private const string SqlQuery = "UPDATE Project SET Deleted = GETUTCDATE() WHERE Id = @Id";

        private readonly ISqlConnectionFactory _connectionFactory;
		private readonly IMediator _mediator;

		public DeleteProjectCommand(ISqlConnectionFactory connectionFactory, IMediator mediator)
		{
			_connectionFactory = connectionFactory;
			_mediator = mediator;
		}

		public async Task<DeleteProjectResponse> Handle(
            [NotNull] DeleteProjectRequest request,
            CancellationToken cancellationToken)
        {
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var project = await connection.ProjectGetById(request.Id);
			if (project == null)
				throw new ResourceNotFoundException($"Project: {request.Id} does not exist");

			await connection.ProjectDelete(request.Id, 1);

            return new();
        }
    }
}
