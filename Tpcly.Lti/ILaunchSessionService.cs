namespace Tpcly.Lti;

public interface ILaunchSessionService
{
    Task<LaunchSession?> Get(string state);

    Task<Tuple<string, LaunchSession>> Start(string state, LtiOpenIdConnectInitiation initiationRequest);
}