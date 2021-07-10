using MediatR;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
        public RatContext Context { get; init; }

        public int UserId { get; init; }

        public string Name { get; set; }
    }
}
