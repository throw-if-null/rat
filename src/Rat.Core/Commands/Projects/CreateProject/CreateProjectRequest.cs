using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Commands.Projects.CreateProject
{
	internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
		public string UserId { get; init; }

        public string Name { get; set; }

        public int ProjectTypeId { get; set; }
	}

    internal static class CreateProjectRequestExtensions
    {
        public static void Validate(this CreateProjectRequest request)
        {
			var validationErrors =
				Validators.ValidateName(request.Name)
				.Union(Validators.ValidateProjectTypeId(request.ProjectTypeId))
				.Union(Validators.ValidateAuthProviderUserId(request.UserId))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
