namespace Rat.Data.Entities
{
    public record User
    {
        public int Id { get; init; }

        public string ExternalId { get; init; }
    }
}
