using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Commands.Projects.CreateProject
{
	internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
        public string Name { get; set; }

        public int ProjectTypeId { get; set; }

		public int CreatedBy { get; init; }
	}

    internal static class CreateProjectRequestExtensions
    {
        public static void Validate(this CreateProjectRequest request)
        {
			var validationErrors =
				Validators.ValidateName(request.Name)
				.Union(Validators.ValidateProjectTypeId(request.ProjectTypeId))
				.Union(ValidateCreatedBy(request.CreatedBy))
				.ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
