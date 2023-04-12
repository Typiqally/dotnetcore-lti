using System.Net.Http.Headers;
using NetCore.Lti.Samples.Spa.Data.Models;

namespace NetCore.Lti.Samples.Spa.Services;

public class CanvasCourseService : ICanvasCourseService
{
    private readonly HttpClient _client;

    public CanvasCourseService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Tuple<HttpResponseMessage, IEnumerable<CanvasCourse>?>> GetCourses()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/courses");
        var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return new Tuple<HttpResponseMessage, IEnumerable<CanvasCourse>?>(response, null);
        }

        return new Tuple<HttpResponseMessage, IEnumerable<CanvasCourse>?>(
            response,
            await response.Content.ReadFromJsonAsync<IEnumerable<CanvasCourse>>()
        );
    }
}