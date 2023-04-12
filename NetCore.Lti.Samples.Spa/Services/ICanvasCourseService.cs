using NetCore.Lti.Samples.Spa.Data.Models;

namespace NetCore.Lti.Samples.Spa.Services;

public interface ICanvasCourseService
{
    Task<Tuple<HttpResponseMessage, IEnumerable<CanvasCourse>?>> GetCourses();
}