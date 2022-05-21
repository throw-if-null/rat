namespace Rat.Api.Auth
{
    public interface IMemberProvider
    {
        Task<int> GetMemberId(CancellationToken ct);
    }
}
