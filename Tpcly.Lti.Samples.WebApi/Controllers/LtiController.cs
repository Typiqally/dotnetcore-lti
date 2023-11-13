using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tpcly.Lti.Samples.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class LtiController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ltiMessage = await HttpContext.GetLtiMessageAsync();
        var info = new Dictionary<string, object?>
        {
            { "lti_version", ltiMessage.Version },
            { "tool", ltiMessage.ToolPlatform },
            { "user_id", ltiMessage.Custom["user_id"] },
            { "course_id", ltiMessage.Custom["course_id"] },
            { "roles" , ltiMessage.Roles },
        };

        return Ok(info);
    }
}