namespace Rat.DataAccess.Views
{
    public record ProjectStatsView
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int TotalConfigurationCount { get; init; }

        public int TotalEntryCount { get; init; }
    }
}
