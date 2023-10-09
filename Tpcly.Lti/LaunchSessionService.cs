using Tpcly.Persistence.Abstractions;

namespace Tpcly.Lti;

public class LaunchSessionService : ILaunchSessionService
{
    private readonly IRepository<LaunchSession> _sessionRepository;

    public LaunchSessionService(IRepository<LaunchSession> sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<LaunchSession?> GetById(string id)
    {
        return await _sessionRepository.FindAsync(id);
    }

    public async Task<LaunchSession> Create(LaunchSession session)
    {
        var created =  await _sessionRepository.CreateAsync(session);
        await _sessionRepository.SaveChangesAsync();

        return created;
    }
}