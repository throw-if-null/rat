using System.Linq;
using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Commands.Projects.DeleteProject
{
	internal record DeleteProjectRequest : IRequest<DeleteProjectResponse>
    {
        internal const string ID_SIGNATURE = nameof(DeleteProjectRequest) + "." + nameof(Id);

        public int Id { get; init; }
    }

    internal static class DeleteProjectRequestExtensions
    {
        public static void Validate(this DeleteProjectRequest request)
        {
            var validationErrors = Validators.ValidateId(request.Id).ToDictionary(x => x.Key, x => x.Value);

			if (validationErrors.Count == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
