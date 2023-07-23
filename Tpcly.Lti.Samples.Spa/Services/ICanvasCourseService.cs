using Tpcly.Lti.Samples.Spa.Data.Models;

namespace Tpcly.Lti.Samples.Spa.Services;

public interface ICanvasCourseService
{
    Task<Tuple<HttpResponseMessage, IEnumerable<CanvasCourse>?>> GetCourses();
}