using MediatR;

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
			var validationErrors = Validators.ValidateId(request.AuthProviderId);

		}
	}
}
