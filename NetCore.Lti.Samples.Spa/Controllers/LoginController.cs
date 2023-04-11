using System.Net.Mime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace NetCore.Lti.Samples.Spa.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    [HttpGet("challenge")]
    public IActionResult ProcessChallenge()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/login/callback",
        };

        return Challenge(properties, Constants.CanvasDockerName);
    }

    [HttpGet("callback")]
    public IActionResult ProcessCallback()
    {
        // Close the login window
        return Content("<script>window.close()</script>", MediaTypeNames.Text.Html);
    }
}