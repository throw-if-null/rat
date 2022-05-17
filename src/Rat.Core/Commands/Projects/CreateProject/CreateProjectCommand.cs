using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.Core.Exceptions;
using Rat.DataAccess;
using Rat.Sql;

namespace Rat.Commands.Projects.CreateProject
{
	internal class CreateProjectCommand : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
	{
		private readonly ISqlConnectionFactory _connectionFactory;

		public CreateProjectCommand(ISqlConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
		{
			request.Validate();

			await using var connection = _connectionFactory.CreateConnection();

			var projectType = await connection.ProjectTypeGetById(request.ProjectTypeId);
			if (projectType == null)
				throw new ResourceNotFoundException($"ProjectType: {request.ProjectTypeId} does not exist");

			var member = await connection.MemberGetByAuthProviderId(request.UserId);
			if (member == null)
				throw new ResourceNotFoundException($"Member: {request.UserId} does not exist");

			int memberId = member.Id;
			var project = await connection.ProjectInsert(request.Name, request.ProjectTypeId, memberId);

			return new()
			{
				Id = project.Id,
				Name = project.Name,
				TypeId = project.ProjectTypeId
			};
		}
	}
}
