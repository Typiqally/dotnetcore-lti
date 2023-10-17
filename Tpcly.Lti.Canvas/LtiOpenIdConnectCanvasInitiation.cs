namespace Tpcly.Lti.Canvas;

public record LtiOpenIdConnectCanvasInitiation(
    string Issuer,
    string LoginHint,
    string ClientId,
    Uri TargetLinkUri,
    string EncodedLtiMessageHint,
    string LtiStorageTarget,
    string CanvasEnvironment,
    string CanvasRegion
) : LtiOpenIdConnectInitiation(Issuer, LoginHint, ClientId, TargetLinkUri, EncodedLtiMessageHint, LtiStorageTarget);