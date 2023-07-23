using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tpcly.Lti.Samples.Spa.Services;

namespace Tpcly.Lti.Samples.Spa.Controllers;

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
        var (response, courses) = await _service.GetCourses();
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }

        return Ok(courses);
    }
}