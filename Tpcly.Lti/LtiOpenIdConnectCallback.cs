namespace Tpcly.Lti;

public record LtiOpenIdConnectCallback(
    string AuthenticityToken,
    string IdToken,
    string State,
    string LtiStorageTarget
);