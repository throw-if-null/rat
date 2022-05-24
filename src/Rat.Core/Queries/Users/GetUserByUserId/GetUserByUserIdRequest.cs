using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Queries.Users.GetUserByUserId
{
	internal record GetUserByUserIdRequest : IRequest<GetUserByUserIdResponse>
	{
		public string AuthProviderId { get; init; }
	}

	internal static class GetUserByUserIdRequestExtensions
	{
		public static void Validate(this GetUserByUserIdRequest request)
		{
			var validationErrors = Validators.ValidateUserId(request.AuthProviderId);

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
