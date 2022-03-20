using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Commands.Projects.CreateProject
{
	internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
        private const string Class_Name = nameof(CreateProject);
        internal const string Name_Signature = Class_Name + "." + nameof(Name);
        internal const string ProjectTypeId_Signature = Class_Name + "." + nameof(ProjectTypeId);
		internal const string UserId_Signature = Class_Name + "." + nameof(UserId);

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
