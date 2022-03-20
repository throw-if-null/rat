using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Commands.Projects.PatchProject
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
    }

    internal static class PatchProjectRequestExtensions
    {
        public static void Validate(this PatchProjectRequest request)
        {
			var validationErrors =
				Validators.ValidateId(request.Id)
				.Union(Validators.ValidateName(request.Name))
				.Union(Validators.ValidateProjectTypeId(request.ProjectTypeId))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
