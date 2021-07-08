namespace Rat.Api.Controllers.Projects.Models
{
    internal record ProjectOverviewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int Entries { get; init; }

        public int Configurations { get; init; }
    }
}
