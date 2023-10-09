using System.Net.Mime;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tpcly.Lti.AspNetCore;

[ApiController]
[Route("/lti")]
public class LtiController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ILtiTokenValidator _tokenValidator;
    private readonly ILaunchSessionService _launchSessionService;
    private readonly IToolPlatformService _toolPlatformService;
    private readonly LtiOptions _ltiOptions;

    public LtiController(
        ILogger<LtiController> logger,
        ILtiTokenValidator tokenValidator,
        ILaunchSessionService launchSessionService,
        IToolPlatformService toolPlatformService,
        IOptions<LtiOptions> options)
    {
        _logger = logger;
        _tokenValidator = tokenValidator;
        _launchSessionService = launchSessionService;
        _toolPlatformService = toolPlatformService;
        _ltiOptions = options.Value;
    }

    [HttpPost("oidc/auth")]
    public async Task<IActionResult> LaunchOidcAuth([ModelBinder(typeof(LtiOpenIdConnectLaunchModelBinder))] LtiOpenIdConnectLaunch launchRequest)
    {
        var origin = Request.Headers.Origin.SingleOrDefault();

        // Currently there is only support for Canvas LMS
        if (launchRequest.Issuer != CanvasConstants.Issuer || origin == null)
        {
            return BadRequest("Invalid issuer");
        }

        var authorizeUrl = new Uri(new Uri(origin), CanvasConstants.AuthorizationEndpoint);
        var hostUrl = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}");

        var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var authorizeRedirectUrl = launchRequest.CreateAuthorizeUrl(
            authorizeUrl,
            new Uri(hostUrl, _ltiOptions.RedirectUri),
            nonce,
            launchRequest.LoginHint
        );

        await _launchSessionService.Create(new LaunchSession
        {
            Id = launchRequest.LoginHint,
            Issuer = launchRequest.Issuer,
            ClientId = launchRequest.ClientId,
            TargetLinkUri = launchRequest.TargetLinkUri,
            LtiStorageTarget = launchRequest.LtiStorageTarget,
            Nonce = nonce,
        });

        _logger.LogDebug("Redirecting launch to {AuthorizeUrl}", authorizeRedirectUrl);

        return Content($"<script>window.location.replace(\"{authorizeRedirectUrl}\")</script>", MediaTypeNames.Text.Html);
    }

    [HttpPost("oidc/callback")]
    public async Task<IActionResult> ProcessOidcCallback([ModelBinder(typeof(LtiOpenIdCallbackLaunchModelBinder))] LtiOpenIdConnectCallback callback)
    {
        var message = new LtiRequest(callback.IdToken);
        var platformReference = message.ToolPlatform;

        var platform = await _toolPlatformService.GetById(platformReference.Id);
        if (platform == null)
        {
            return NotFound("Unable to find platform");
        }

        var signatureResult = await _tokenValidator.ValidateSignature(platform, message);
        if (signatureResult == null)
        {
            return BadRequest("Unable to verify identity token");
        }

        var session = await _launchSessionService.GetById(callback.State);
        if (session == null)
        {
            return BadRequest("Unable to determine session");
        }

        var nonce = message.Payload.Nonce;
        if (nonce == null || !Equals(nonce, session.Nonce))
        {
            return BadRequest("Nonce mismatch");
        }
        
        _logger.LogDebug("Redirecting callback to {CallbackUrl}", message.TargetLinkUri);
        return Redirect(message.TargetLinkUri.ToString());
    }
}