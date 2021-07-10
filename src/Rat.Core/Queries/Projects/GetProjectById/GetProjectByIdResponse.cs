namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdResponse
    {
        public RatContext Context { get; init; }

        public int Id { get; init; }

        public int UserId { get; init; }

        public string Name { get; init; }
    }
}
