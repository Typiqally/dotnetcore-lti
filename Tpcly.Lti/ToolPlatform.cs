using NetCore.Persistence;

namespace Tpcly.Lti;

public record ToolPlatform : Entity<string>
{
    public string? Name { get; set; }
    public string? Issuer { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public Uri? AccessTokenUrl { get; set; }
    public Uri? AuthorizeUrl { get; set; }
    public Uri? JwkSetUrl { get; set; }
    public string? KeyId { get; set; }
}