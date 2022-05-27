using MediatR;
using Rat.Core;
using Rat.Core.Exceptions;

namespace Rat.Queries.Projects.GetProjectsForUser
{
	internal record GetProjectsForUserRequest : IRequest<GetProjectsForUserResponse>
    {
        public RatContext Context { get; init; } = new();

        public int MemberId { get; init; }
    }

    internal static class GetProjectsForUserRequestExtensions
    {
        public static void Validate(this GetProjectsForUserRequest request)
        {
			var validationErrors = Validators.ValidateMemberId(request.MemberId);

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
    }
}
