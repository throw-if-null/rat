using MediatR;
using Rat.Core.Exceptions;

namespace Rat.Queries.Projects.GetProjectById
{
	internal record GetProjectByIdRequest : IRequest<GetProjectByIdResponse>
    {
        internal const string ID_SIGNATURE = nameof(GetProjectByIdRequest) + "." + nameof(Id);

        public int Id { get; init; }
    }

    internal static class GetProjectByIdRequestExtensions
    {
        public static void Validate(this GetProjectByIdRequest request)
        {
            var validationErrors = Validators.ValidateId(request.Id);

			if (validationErrors.Length == 0)
				return;

			throw new InvalidRequestDataException(validationErrors);
        }
    }
}
