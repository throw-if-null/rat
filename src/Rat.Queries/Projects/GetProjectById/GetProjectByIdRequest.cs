using MediatR;
using Rat.Core;

namespace Rat.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdRequest : IRequest<GetProjectByIdResponse>
    {
        internal const string ID_SIGNATURE = nameof(GetProjectByIdRequest) + "." + nameof(Id);

        public int Id { get; init; }

        public RatContext Context { get; init; } = new();
    }

    internal static class GetProjectByIdRequestExtensions
    {
        public static void Validate(this GetProjectByIdRequest request)
        {
            Validators.ValidateId(request.Id, request.Context);

            Validators.MakeGoodOrBad(request.Context);
        }
    }
}
