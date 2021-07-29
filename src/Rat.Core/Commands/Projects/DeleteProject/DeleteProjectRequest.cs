using MediatR;
using Rat.Core.Properties;

namespace Rat.Core.Commands.Projects.DeleteProject
{
    internal record DeleteProjectRequest : IRequest<DeleteProjectResponse>
    {
        internal const string ID_SIGNATURE = nameof(DeleteProjectRequest) + "." + nameof(Id);

        public int Id { get; init; }

        public RatContext Context { get; init; } = new();
    }

    internal static class DeleteProjectRequestExtensions
    {
        public static void Validate(this DeleteProjectRequest request)
        {
            if (request.Id <= 0)
            {
                request.Context.ValidationErrors.Add(
                    DeleteProjectRequest.ID_SIGNATURE,
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
