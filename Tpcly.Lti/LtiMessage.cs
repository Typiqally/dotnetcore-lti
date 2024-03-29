using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Tpcly.Lti.Extensions;

namespace Tpcly.Lti;

public class LtiMessage : JwtSecurityToken
{
    public LtiMessage(string idToken) : base(idToken)
    {
    }

    public string? Type => Claims.GetValue(LtiClaimType.MessageType);

    public string? Version => Claims.GetValue(LtiClaimType.Version);

    public string? DeploymentId => Claims.GetValue(LtiClaimType.DeploymentId);

    public ResourceLink? ResourceLink => Claims.GetValue<ResourceLink>(LtiClaimType.ResourceLink);

    public Uri TargetLinkUri => new(Claims.GetValue(LtiClaimType.TargetLinkUri) ?? string.Empty);

    public LearningInformationServices? Lis => Claims.GetValue<LearningInformationServices>(LtiClaimType.LearningInformationServices);

    public Context? Context => Claims.GetValue<Context>(LtiClaimType.Context);

    public ToolPlatformReference? ToolPlatform => Claims.GetValue<ToolPlatformReference>(LtiClaimType.ToolPlatform);

    public LaunchPresentation? LaunchPresentation => Claims.GetValue<LaunchPresentation>(LtiClaimType.LaunchPresentation);

    public IEnumerable<string>? Roles => Claims.GetValues(LtiClaimType.Roles);

    public IDictionary<string, object>? Custom => Claims.GetValue<IDictionary<string, object>?>(LtiClaimType.Custom);

    public JsonWebKey GetSigningKey(JsonWebKeySet jwks) => jwks.Keys.Single(k => k.Kid == Header.Kid.Trim('"'));
}