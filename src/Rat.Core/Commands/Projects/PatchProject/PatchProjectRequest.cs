using MediatR;
using Rat.Data.Entities;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectRequest : IRequest<PatchProjectResponse>
    {
        private const string Class_Name = nameof(PatchProjectRequest) + ".";
        internal const string Id_Signature = Class_Name + nameof(Id);
        internal const string Name_Signature = Class_Name + nameof(Name);
        internal const string ProjectTypeId_Signature = Class_Name + nameof(ProjectTypeId);

        public int Id { get; init; }

        public string Name { get; init; }

        public int ProjectTypeId { get; init; }

        public RatContext Context { get; init; } = new();
    }

    internal static class PatchProjectRequestExtensions
    {
        public static void Validate(this PatchProjectRequest request, ProjectTypeEntity projectType)
        {
            Validators.ValidateId(request.Id, request.Context);
            Validators.ValidateName(request.Name, request.Context);
            Validators.ValidateProjectType(projectType, request.Context);

            Validators.MakeGoodOrBad(request.Context);
        }
    }
}
