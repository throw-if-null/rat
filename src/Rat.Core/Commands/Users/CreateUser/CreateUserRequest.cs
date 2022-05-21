using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Commands.Users.CreateUser
{
	internal record CreateUserRequest : IRequest<CreateUserResponse>
	{
		public string AuthProviderId { get; init; }
	}

	internal static class CreateUserRequestExtensions
	{
		public static void Validate(this CreateUserRequest request)
		{
			var validationErrors = Validators.ValidateAuthProviderId(request.AuthProviderId);

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
		}
	}
}
