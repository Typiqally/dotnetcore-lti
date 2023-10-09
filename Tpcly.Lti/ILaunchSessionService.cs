namespace Tpcly.Lti;

public interface ILaunchSessionService
{
    Task<LaunchSession?> GetById(string id);
    
    Task<LaunchSession> Create(LaunchSession session);
}