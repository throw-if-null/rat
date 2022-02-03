namespace Rat.Api.Observability.Health.Responses
{
    public record HealthCheckInfo
    {
        public string Name { get; init; }

        public HealthCheckData Report { get; init; }
    }
}
