using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Tpcly.Lti;

public interface ILtiTokenValidator
{
     Task<Tuple<ClaimsPrincipal, SecurityToken>?> ValidateSignature(ToolPlatform toolPlatform, LtiRequest request, TokenValidationParameters? validationParameters = null);
}