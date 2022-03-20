using MediatR;

namespace Rat.Commands.Users.CreateUser
{
	internal record CreateUserRequest : IRequest<CreateUserResponse>
	{
		internal const string AuthProviderUserIdSignature = nameof(CreateUserRequest) + "." + nameof(AuthProviderUserId);

		public string AuthProviderUserId { get; init; }
	}

	internal static class CreateUserRequestExtensions
	{
		public static void Validate(this CreateUserRequest request)
		{
			var validationErrors = Validators.ValidateId(request.AuthProviderUserId);

		}
	}
}
