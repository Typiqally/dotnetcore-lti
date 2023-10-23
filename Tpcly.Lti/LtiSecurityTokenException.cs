using Microsoft.IdentityModel.Tokens;

namespace Tpcly.Lti;

public class LtiSecurityTokenException : SecurityTokenException
{
    public LtiSecurityTokenException(string message) : base(message)
    {
    }
}