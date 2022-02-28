using Rat.Core;
using Rat.Data.Views;

namespace Rat.Commands.Projects.CreateProject
{
    internal record CreateProjectResponse
    {
        public RatContext Context { get; init; }

		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }
	}
}
