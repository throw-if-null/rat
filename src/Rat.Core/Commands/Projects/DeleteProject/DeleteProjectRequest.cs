using System.Linq;
using MediatR;
using Rat.Core.Exceptions;
using static Rat.Core.Validators;

namespace Rat.Commands.Projects.DeleteProject
{
	internal record DeleteProjectRequest : IRequest<DeleteProjectResponse>
    {
        public int Id { get; init; }

		public int DeletedBy { get; init; }
    }

    internal static class DeleteProjectRequestExtensions
    {
        public static void Validate(this DeleteProjectRequest request)
        {
            var validationErrors =
				ValidateId(request.Id).ToDictionary(x => x.Key, x => x.Value)
				.Union(ValidateDeletedBy(request.DeletedBy)).ToArray();

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
