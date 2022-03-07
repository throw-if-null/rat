namespace Rat.Commands.Projects.CreateProject
{
	internal record CreateProjectResponse
    {

		public int Id { get; init; }

		public string Name { get; init; }

		public int TypeId { get; init; }
	}
}
