namespace Rat.Core.Commands.Projects.PatchProject
{
    internal record PatchProjectResponse
    {
        public RatContext Context { get; init; }

        public int Id { get; init; }

        public int UserId { get; init; }

        public string Name { get; init; }
    }
}
