namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectResponse
    {
        public RatContext Context { get; init; }

        public int Id { get; init; }

        public int UserId { get; init; }

        public string Name { get; init; }
    }
}
