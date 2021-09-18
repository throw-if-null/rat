using MediatR;
using Rat.Data.Entities;

namespace Rat.Core.Commands.Projects.CreateProject
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

        public RatContext Context { get; init; } = new();
	}

    internal static class CreateProjectRequestExtensions
    {
        public static void Validate(this CreateProjectRequest request, ProjectTypeEntity projectType, UserEntity user)
        {
            Validators.ValidateName(request.Name, request.Context);
            Validators.ValidateProjectType(projectType, request.Context);
			Validators.ValidateUser(user, request.Context);

            Validators.MakeGoodOrBad(request.Context);
        }
    }
}
