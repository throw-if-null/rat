using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Rat.Core.Exceptions;
using Rat.Core.Properties;
using Rat.DataAccess;
using Rat.Queries.Projects.GetProjectById;

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

			var getProjectByIdResponse = await _mediator.Send(new GetProjectByIdRequest { Id = request.Id }, cancellationToken);

            if (getProjectByIdResponse == null)
				throw new ResourceNotFoundException($"Project: {request.Id} does not exist");

			await using var connection = _connectionFactory.CreateConnection();

			var command = new CommandDefinition(
				SqlQuery,
				new { Id = request.Id },
				cancellationToken: cancellationToken);

			var changes = await connection.ExecuteAsync(command);

            var expectedNumberOfChanges = 1;

            if (changes != expectedNumberOfChanges)
                throw new RatDbException(string.Format(Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch, changes, expectedNumberOfChanges));

            return new();
        }
    }
}
