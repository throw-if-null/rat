using MediatR;

namespace Rat.Commands.Users.CreateUser
{
	internal record CreateUserRequest : IRequest<CreateUserResponse>
	{
		internal const string AuthProviderUserIdSignature = nameof(CreateUserRequest) + "." + nameof(AuthProviderId);

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
