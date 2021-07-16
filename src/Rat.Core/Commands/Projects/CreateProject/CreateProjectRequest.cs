using MediatR;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
        public RatContext Context { get; init; } = new();

        public int UserId { get; init; }

        public string Name { get; set; }

        public int ProjectTypeId { get; set; }
    }
}
