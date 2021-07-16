namespace Rat.Api.Controllers.Projects.Models
{
    public record PatchProjectModel
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public int TypeId { get; init; }
    }
}
