using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Commands.Projects.PatchProject
{
	internal record PatchProjectRequest : IRequest<PatchProjectResponse>
    {
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
