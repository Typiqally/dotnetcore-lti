namespace Tpcly.Lti;

public interface ILaunchSessionService
{
    Task<LaunchSession?> Get(string state);
    
    Task<Tuple<string, LaunchSession>> Start(LtiOpenIdConnectLaunch launchRequest);

    Task<Tuple<string, LaunchSessionCredentials>> StoreCredentials(LaunchSession session, string identityToken);

    Task<LaunchSessionCredentials?> ExchangeToken(string token, string state);
}