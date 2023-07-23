using IdentityModel.Jwk;

namespace Tpcly.Lti;

public class LtiOptions
{
    public string RedirectUri { get; set; }
    
    public JsonWebKey Jwk { get; set; }
}