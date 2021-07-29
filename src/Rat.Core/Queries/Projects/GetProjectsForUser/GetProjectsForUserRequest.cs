using MediatR;
using Rat.Core.Properties;

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
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                request.Context.ValidationErrors.Add(
                    GetProjectsForUserRequest.UserId_Signature,
                    Resources.MustNotBeNullOrEmpty);

                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = Resources.BadRequest;
            }
            else
            {
                request.Context.Status = ProcessingStatus.GoodRequest;
            }
        }
    }
}
