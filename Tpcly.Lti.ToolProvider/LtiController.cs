using System.Net.Mime;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tpcly.Lti.Canvas;

namespace Tpcly.Lti.ToolProvider;

[ApiController]
[Route("/lti")]
public class LtiController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ILaunchSessionService _launchSessionService;
    private readonly IToolPlatformService _toolPlatformService;
    private readonly LtiToolProviderOptions _toolProviderOptions;

    public LtiController(
        ILogger<LtiController> logger,
        ILaunchSessionService launchSessionService,
        IToolPlatformService toolPlatformService,
        IOptions<LtiToolProviderOptions> toolProviderOptions)
    {
        _logger = logger;
        _launchSessionService = launchSessionService;
        _toolPlatformService = toolPlatformService;
        _toolProviderOptions = toolProviderOptions.Value;
    }

    [HttpPost("oidc/auth")]
    public async Task<IActionResult> LaunchOidcAuth([ModelBinder(typeof(LtiOpenIdConnectLaunchModelBinder))] LtiOpenIdConnectInitiation initiationRequest)
    {
        var origin = Request.Headers.Origin.SingleOrDefault();

        // Currently there is only support for Canvas LMS
        if (initiationRequest.Issuer != CanvasConstants.Issuer || origin == null)
        {
            return BadRequest("Invalid issuer");
        }

        var authorizeUrl = new Uri(new Uri(origin), CanvasConstants.AuthorizationEndpoint);
        var hostUrl = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}");

        var state = Base64UrlTextEncoder.Encode(RandomNumberGenerator.GetBytes(32));
        var (nonce, _) = await _launchSessionService.Start(state, initiationRequest);
        var authorizeRedirectUrl = initiationRequest.CreateAuthorizeUrl(
            authorizeUrl,
            new Uri(hostUrl, _toolProviderOptions.RedirectUri),
            nonce,
            state
        );

        _logger.LogDebug("Redirecting launch to {AuthorizeUrl}", authorizeRedirectUrl);

        if (Request.Headers.UserAgent.ToString()!.Contains("Safari"))
        {
            return Content($"<script>window.location.replace('{authorizeRedirectUrl}')</script>", MediaTypeNames.Text.Html);
        }

        return Redirect(authorizeRedirectUrl);
    }

    [HttpPost("oidc/callback")]
    public async Task<IActionResult> ProcessOidcCallback([ModelBinder(typeof(LtiOpenIdCallbackLaunchModelBinder))] LtiOpenIdConnectCallback callback)
    {
        var message = new LtiMessage(callback.IdToken);

        var toolPlatformReference = message.ToolPlatform;
        if (toolPlatformReference == null)
        {
            return BadRequest("LTI request does not contain tool platform reference");
        }

        var platform = await _toolPlatformService.GetById(toolPlatformReference.Id);
        if (platform == null)
        {
            return NotFound($"Unable to find platform {toolPlatformReference.Id}");
        }

        var session = await _launchSessionService.Get(callback.State);
        if (session == null)
        {
            return BadRequest("Unable to determine session");
        }

        var nonce = message.Payload.Nonce;
        if (nonce == null || !Equals(nonce, session.Nonce))
        {
            return BadRequest("Nonce mismatch");
        }

        var (token, _) = await _launchSessionService.StoreCredentials(session, callback.IdToken);

        var builder = new UriBuilder(message.TargetLinkUri.ToString())
        {
            Query = $"token={token}",
        };

        var redirectUri = builder.ToString();

        _logger.LogDebug("Redirecting callback to {RedirectUri}", redirectUri);
        return Redirect(redirectUri);
    }

    [HttpPost("exchange")]
    public async Task<IActionResult> ExchangeToken([FromBody] LaunchSessionExchangeRequest exchangeRequest)
    {
        var credentials = await _launchSessionService.ExchangeToken(exchangeRequest.Token);
        if (credentials == null)
        {
            return BadRequest("Invalid token");
        }
        
        return Ok(credentials);
    }
}