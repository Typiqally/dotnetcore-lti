using System.Text.Json.Serialization;
using Tpcly.Persistence;

namespace Tpcly.Lti;

public record LaunchSessionCredentials(
    string Token,
    LaunchSession Session,
    string IdentityToken
) : Entity<Guid>
{
    [JsonIgnore]
    public string Token { get; set; } = Token;

    [JsonIgnore]
    public LaunchSession Session { get; set; } = Session;
    
    public string IdentityToken { get; set; } = IdentityToken;

    public bool Used { get; set; } = false;

    public DateTime? UsedAt { get; set; } = null;

    public DateTime CreatedAt { get; } = DateTime.Now;
}