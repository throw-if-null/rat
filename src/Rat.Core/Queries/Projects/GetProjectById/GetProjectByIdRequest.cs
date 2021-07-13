using MediatR;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdRequest : IRequest<GetProjectByIdResponse>
    {
        public RatContext Context { get; init; } = new();

        public int Id { get; init; }
    }
}
