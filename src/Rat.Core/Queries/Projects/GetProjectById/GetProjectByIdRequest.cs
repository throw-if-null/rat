using MediatR;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdRequest : IRequest<GetProjectByIdResponse>
    {
        public RatContext Context { get; init; }

        public int Id { get; init; }
    }
}
