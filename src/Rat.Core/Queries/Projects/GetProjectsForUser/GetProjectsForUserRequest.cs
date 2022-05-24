using MediatR;
using Rat.Core;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal record GetProjectsForUserRequest : IRequest<GetProjectsForUserResponse>
    {
        internal const string UserId_Signature = nameof(GetProjectsForUserRequest) + "." + nameof(MemberId);

        public RatContext Context { get; init; } = new();

        public int MemberId { get; init; }
    }

    internal static class GetProjectsForUserRequestExtensions
    {
        public static void Validate(this GetProjectsForUserRequest request)
        {
			return;
        }
    }
}
