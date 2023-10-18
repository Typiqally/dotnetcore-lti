using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Tpcly.Lti;

public record LtiOpenIdConnectInitiation(
    string Issuer,
    string LoginHint,
    string ClientId,
    Uri TargetLinkUri,
    string EncodedLtiMessageHint,
    string LtiStorageTarget
)
{
    public string CreateAuthorizeUrl(Uri baseUrl, Uri redirectUri, string nonce, string? state = null)
    {
        var openIdMessage = new OpenIdConnectMessage
        {
            IssuerAddress = baseUrl.ToString(),
            ClientId = ClientId,
            ResponseType = OpenIdConnectResponseType.IdToken,
            ResponseMode = OpenIdConnectResponseMode.FormPost,
            Scope = OpenIdConnectScope.OpenId,
            RedirectUri = redirectUri.ToString(),
            State = state,
            Nonce = nonce,
            LoginHint = LoginHint,
            Prompt = OpenIdConnectPrompt.None,
            Parameters =
            {
                { "lti_message_hint", EncodedLtiMessageHint }
            }
        };

        return openIdMessage.CreateAuthenticationRequestUrl();
    }
}