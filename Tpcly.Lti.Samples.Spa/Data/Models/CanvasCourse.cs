using System.Text.Json.Serialization;

namespace Tpcly.Lti.Samples.Spa.Data.Models;

public record CanvasCourse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name
);