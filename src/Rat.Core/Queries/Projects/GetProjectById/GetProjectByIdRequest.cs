using MediatR;
using Rat.Core.Commands.Projects;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdRequest : IRequest<GetProjectByIdResponse>
    {
        internal const string Id_Signature = nameof(GetProjectByIdRequest) + "." + nameof(Id);

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
