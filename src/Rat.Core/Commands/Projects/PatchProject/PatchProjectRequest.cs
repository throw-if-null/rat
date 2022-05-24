using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Commands.Validators;

namespace Rat.Commands.Projects.PatchProject
{
	internal record PatchProjectRequest : IRequest<PatchProjectResponse>
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int ProjectTypeId { get; init; }

		public int ModifiedBy { get; init; }
    }

    internal static class PatchProjectRequestExtensions
    {
        public static void Validate(this PatchProjectRequest request)
        {
			var validationErrors =
				ValidateId(request.Id)
				.Union(ValidateModifiedBy(request.ModifiedBy))
				.Union(Validators.ValidateName(request.Name))
				.Union(Validators.ValidateProjectTypeId(request.ProjectTypeId))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
