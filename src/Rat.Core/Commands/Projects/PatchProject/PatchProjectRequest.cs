using MediatR;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectRequest : IRequest<PatchProjectResponse>
    {
        public RatContext Context { get; init; } = new();

        public int Id { get; set; }

        public string Name { get; init; }
    }
}
