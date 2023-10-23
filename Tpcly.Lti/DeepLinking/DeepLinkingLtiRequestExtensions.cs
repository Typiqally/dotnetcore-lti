using Tpcly.Lti.Extensions;

namespace Tpcly.Lti.DeepLinking;

public static class DeepLinkingLtiRequestExtensions
{
    public static DeepLinkingSettings? DeepLinkingSettings(this LtiMessage message)
    {
        return message.Claims.GetValue<DeepLinkingSettings>(LtiDeepLinkingClaimType.DeepLinkingSettings);
    }
}