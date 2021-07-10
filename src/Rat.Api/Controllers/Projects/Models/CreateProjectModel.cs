namespace Rat.Api.Controllers.Projects.Models
{
    public record CreateProjectModel
    {
        public string Name { get; init; }

        public string Type { get; init; }
    }
}
