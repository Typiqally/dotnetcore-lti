using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tpcly.Lti.Extensions;
using Tpcly.Persistence.Abstractions;

namespace Tpcly.Lti;

public class LaunchSessionService : ILaunchSessionService
{
    private readonly ILogger<LaunchSessionService> _logger;
    private readonly LtiOptions _options;
    private readonly IRepository<LaunchSession> _sessionRepository;
    private readonly IRepository<LaunchSessionCredentials> _sessionTokenRepository;
    private readonly JwtSecurityTokenHandler _jwtHandler;

    public LaunchSessionService(
        ILogger<LaunchSessionService> logger,
        IOptions<LtiOptions> options,
        IRepository<LaunchSession> sessionRepository,
        IRepository<LaunchSessionCredentials> sessionTokenRepository
    )
    {
        _logger = logger;
        _options = options.Value;
        _sessionRepository = sessionRepository;
        _sessionTokenRepository = sessionTokenRepository;
        _jwtHandler = new JwtSecurityTokenHandler();
    }

    public async Task<LaunchSession?> Get(string state)
    {
        return await _sessionRepository.FindAsync(state);
    }

    public async Task<Tuple<string, LaunchSession>> Start(string state, LtiOpenIdConnectLaunch launchRequest)
    {
        var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var session = new LaunchSession
        {
            Id = state,
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
        var token = new JwtSecurityToken(
            claims: new[]
            {
                new Claim("state", session.Id)
            },
            expires: DateTime.UtcNow.AddSeconds(5),
            signingCredentials: new SigningCredentials(_options.Jwk.ToRsaSecurityKey(), SecurityAlgorithms.RsaSha256)
        );

        var jwt = _jwtHandler.WriteToken(token);

        var credentials = new LaunchSessionCredentials(jwt, session, identityToken);

        var createdToken = await _sessionTokenRepository.CreateAsync(credentials);
        await _sessionTokenRepository.SaveChangesAsync();

        return new Tuple<string, LaunchSessionCredentials>(jwt, createdToken);
    }

    public async Task<LaunchSessionCredentials?> ExchangeToken(string token, TokenValidationParameters? validationParameters = null)
    {
        var credentials = await _sessionTokenRepository.SingleOrDefaultAsync(
            c => c.Token == token && !c.Used,
            new[] { nameof(LaunchSessionCredentials.Session) }
        );

        if (credentials == null)
        {
            _logger.LogDebug("Unable to find credentials for token");
            return null;
        }

        validationParameters ??= new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = _options.Jwk.ToRsaSecurityKey(),
        };

        var claimsPrincipal = _jwtHandler.ValidateToken(token, validationParameters, out var validatedToken);
        if (DateTime.UtcNow > validatedToken.ValidTo)
        {
            _logger.LogDebug("Token has expired");
            return null;
        }

        var state = claimsPrincipal.FindFirst("state")?.Value;
        if (credentials.Session.Id != state)
        {
            _logger.LogDebug("State mismatch");
            return null;
        }

        credentials.Used = true;
        credentials.UsedAt = DateTime.Now;

        _sessionTokenRepository.Update(credentials);
        await _sessionTokenRepository.SaveChangesAsync();

        return credentials;
    }
}