namespace Rat.Data.Views
{
    public record ProjectView
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int TypeId { get; init; }
    }
}