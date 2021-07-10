using MediatR;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserRequest : IRequest<GetProjectsForUserResponse>
    {
        public RatContext Context { get; init; }

        public int UserId { get; init; }
    }
}
