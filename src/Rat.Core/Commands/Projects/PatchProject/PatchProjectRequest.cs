using MediatR;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectRequest : IRequest<PatchProjectResponse>
    {
        public RatContext Context { get; init; } = new();

        public int Id { get; init; }

        public string Name { get; init; }

        public int ProjectTypeId { get; init; }
    }
}
