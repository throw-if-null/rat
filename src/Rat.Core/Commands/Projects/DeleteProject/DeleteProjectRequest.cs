using MediatR;

namespace Rat.Core.Commands.Projects.DeleteProject
{
    internal record DeleteProjectRequest : IRequest<DeleteProjectResponse>
    {
        public RatContext Context { get; init; } = new();

        public int Id { get; init; }
    }
}
