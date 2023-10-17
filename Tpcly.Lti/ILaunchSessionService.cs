using Microsoft.IdentityModel.Tokens;

namespace Tpcly.Lti;

public interface ILaunchSessionService
{
    Task<LaunchSession?> Get(string state);

    Task<Tuple<string, LaunchSession>> Start(string state, LtiOpenIdConnectLaunch launchRequest);

    Task<Tuple<string, LaunchSessionCredentials>> StoreCredentials(LaunchSession session, string identityToken);

    Task<LaunchSessionCredentials?> ExchangeToken(string token, TokenValidationParameters? validationParameters = null);
}