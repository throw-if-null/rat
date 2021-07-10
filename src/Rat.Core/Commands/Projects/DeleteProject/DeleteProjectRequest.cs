using MediatR;

namespace Rat.Core.Commands.Projects.DeleteProject
{
    internal record DeleteProjectRequest : IRequest<DeleteProjectResponse>
    {
        public RatContext Context { get; init; }

        public int Id { get; init; }
    }
}
