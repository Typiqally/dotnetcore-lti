using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Lti.Samples.Spa.Services;

namespace NetCore.Lti.Samples.Spa.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICanvasCourseService _service;

    public CourseController(ICanvasCourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var token = await HttpContext.GetTokenAsync(Constants.CanvasDockerName, "access_token");
        var (response, courses) = await _service.GetCourses(token);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }

        return Ok(courses);
    }
}