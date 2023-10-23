using Microsoft.IdentityModel.Tokens;

namespace Tpcly.Lti;

public class LtiToolProviderOptions
{
    public string RedirectUri { get; set; }
    
    public JsonWebKey Jwk { get; set; }
}