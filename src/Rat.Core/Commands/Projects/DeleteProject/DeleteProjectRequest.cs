using MediatR;

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
            Validators.ValidateId(request.Id, request.Context);

            Validators.MakeGoodOrBad(request.Context);
        }
    }
}
