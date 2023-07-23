using System.Text.Json.Serialization;

namespace Tpcly.Lti;

public record ResourceLink(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title
)
{
    [JsonPropertyName("description")]
    public string? Description { get; }
}