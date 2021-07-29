using MediatR;
using Rat.Core.Commands.Projects;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserRequest : IRequest<GetProjectsForUserResponse>
    {
        internal const string UserId_Signature = nameof(GetProjectsForUserRequest) + "." + nameof(UserId);

        public RatContext Context { get; init; } = new();

        public string UserId { get; init; }
    }

    internal static class GetProjectsForUserRequestExtensions
    {
        public static void Validate(this GetProjectsForUserRequest request)
        {
            Validators.ValidateUserId(request.UserId, request.Context);

            Validators.MakeGoodOrBad(request.Context);
        }
    }
}
