using Tpcly.Persistence;

namespace Tpcly.Lti;

public record LaunchSession : Entity<string>
{
    public string Issuer { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public Uri TargetLinkUri { get; set; } = null!;
    public string? LtiStorageTarget { get; set; }
    public string Nonce { get; set; } = null!;
    public DateTime CreatedAt = DateTime.Now;
}