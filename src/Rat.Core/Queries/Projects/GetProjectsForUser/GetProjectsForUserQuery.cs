using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rat.DataAccess.Projects;
using Rat.DataAccess.Users;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal class GetProjectsForUserQuery : IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public GetProjectsForUserQuery(IProjectRepository repository, IUserRepository userRepository)
        {
            _projectRepository = repository;
            _userRepository = userRepository;
        }

        public async Task<GetProjectsForUserResponse> Handle(GetProjectsForUserRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                request.Context.ValidationErrors.Add(
                    $"{nameof(GetProjectsForUserRequest)}.{nameof(GetProjectsForUserRequest.UserId)}",
                    "UserId must be provided");

                request.Context.Status = ProcessingStatus.BadRequest;

                return new() { Context = request.Context };
            }

            var user = await _userRepository.Retrieve(request.UserId, cancellationToken);

            if (user == null)
            {
                request.Context.Status = ProcessingStatus.NotFound;

                return new() { Context = request.Context };
            }

            var userProjectStats = await _projectRepository.RetrieveUserProjectStats(user.Id, cancellationToken);

            request.Context.Status = ProcessingStatus.Ok;

            return new() { Context = request.Context, UserProjectStats = userProjectStats };
        }
    }
}
