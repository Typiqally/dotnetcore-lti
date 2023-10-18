using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Tpcly.Lti;

public class LtiSecurityTokenValidator : ISecurityTokenValidator
{
    private readonly IToolPlatformService _toolPlatformService;
    private readonly JwtSecurityTokenHandler _jwtHandler;

    public LtiSecurityTokenValidator(IToolPlatformService toolPlatformService)
    {
        _toolPlatformService = toolPlatformService;
        _jwtHandler = new JwtSecurityTokenHandler();
    }

    public bool CanReadToken(string securityToken)
    {
        return _jwtHandler.CanReadToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        var task = Task.Run(async () =>
        {
            var message = new LtiMessage(securityToken);

            var toolPlatformReference = message.ToolPlatform;
            if (toolPlatformReference == null)
            {
                throw new LtiSecurityTokenException("Tool platform could not be found");
            }

            var toolPlatform = await _toolPlatformService.GetById(toolPlatformReference.Id);
            if (toolPlatform == null)
            {
                throw new LtiSecurityTokenException("Tool platform could not be found");
            }

            var platformJwks = await _toolPlatformService.GetJwks(toolPlatform);
            if (platformJwks == null)
            {
                throw new LtiSecurityTokenException("JSON Web Key Sets could not be fetched for tool platform");
            }

            return new Tuple<ToolPlatform, JsonWebKeySet>(toolPlatform, platformJwks);
        });
        
        var (toolPlatform, jwks) = task.Result;
        
        validationParameters.ValidAudience = toolPlatform.ClientId;
        validationParameters.ValidIssuer = toolPlatform.Issuer;
        validationParameters.IssuerSigningKeys = jwks.GetSigningKeys();
        
        var claimsPrincipal = _jwtHandler.ValidateToken(securityToken, validationParameters, out var validatedSecurityToken);
        validatedToken = validatedSecurityToken;
        
        return claimsPrincipal;
    }

    public bool CanValidateToken { get; }

    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
}