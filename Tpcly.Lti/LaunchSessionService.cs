using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Tpcly.Persistence.Abstractions;

namespace Tpcly.Lti;

public class LaunchSessionService : ILaunchSessionService
{
    private readonly IRepository<LaunchSession> _sessionRepository;
    private readonly IRepository<LaunchSessionCredentials> _sessionTokenRepository;

    public LaunchSessionService(IRepository<LaunchSession> sessionRepository, IRepository<LaunchSessionCredentials> sessionTokenRepository)
    {
        _sessionRepository = sessionRepository;
        _sessionTokenRepository = sessionTokenRepository;
    }

    public async Task<LaunchSession?> Get(string state)
    {
        return await _sessionRepository.FindAsync(state);
    }

    public async Task<Tuple<string, LaunchSession>> Start(LtiOpenIdConnectLaunch launchRequest)
    {
        var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var session = new LaunchSession
        {
            Id = launchRequest.LoginHint,
            Issuer = launchRequest.Issuer,
            ClientId = launchRequest.ClientId,
            TargetLinkUri = launchRequest.TargetLinkUri,
            LtiStorageTarget = launchRequest.LtiStorageTarget,
            Nonce = nonce,
        };

        var createdSession = await _sessionRepository.CreateAsync(session);
        await _sessionRepository.SaveChangesAsync();

        return new Tuple<string, LaunchSession>(nonce, createdSession);
    }

    public async Task<Tuple<string, LaunchSessionCredentials>> StoreCredentials(LaunchSession session, string identityToken)
    {
        var oneTimeToken = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32));
        var token = new LaunchSessionCredentials(oneTimeToken, session, identityToken);

        var createdToken = await _sessionTokenRepository.CreateAsync(token);
        await _sessionTokenRepository.SaveChangesAsync();

        return new Tuple<string, LaunchSessionCredentials>(oneTimeToken, createdToken);
    }

    public async Task<LaunchSessionCredentials?> ExchangeToken(string token, string state)
    {
        var credentials = await _sessionTokenRepository.SingleOrDefaultAsync(
            c => c.Token == token && !c.Used,
            new[] { nameof(LaunchSessionCredentials.Session) }
        );

        if (credentials == null || credentials.Session.Id != state)
        {
            return null;
        }

        credentials.Used = true;
        credentials.UsedAt = DateTime.Now;

        _sessionTokenRepository.Update(credentials);
        await _sessionTokenRepository.SaveChangesAsync();

        return credentials;
    }
}