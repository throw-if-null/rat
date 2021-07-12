namespace Rat.Data.Views
{
    public record ProjectStats
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int TotalConfigurationCount { get; init; }

        public int TotalEntryCount { get; init; }
    }
}
