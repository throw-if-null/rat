using MediatR;
using Rat.Core;
using Rat.Data.Exceptions;

namespace Rat.Queries.Projects.GetProjectsForUser
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
            var validationErrors = Validators.ValidateUserId(request.UserId);

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
