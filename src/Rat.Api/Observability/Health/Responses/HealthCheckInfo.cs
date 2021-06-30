namespace Rat.Api.Observability.Health.Responses
{
    public class HealthCheckInfo
    {
        public string Name { get; set; }

        public HealthCheckData Report { get; set; }
    }
}
