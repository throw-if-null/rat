using MediatR;
using Rat.Core.Properties;

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
            if (request.Id <= 0)
            {
                request.Context.ValidationErrors.Add(
                    GetProjectByIdRequest.Id_Signature,
                    Resources.IdMustBeLargerThenZero);

                request.Context.Status = ProcessingStatus.BadRequest;
                request.Context.FailureReason = Resources.BadRequest;
            }
            else
            {
                request.Context.Status = ProcessingStatus.GoodRequest;
            }
        }
    }
}
