using Rat.Data.Views;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal record GetProjectByIdResponse
    {
        public RatContext Context { get; init; }

		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }
	}
}
