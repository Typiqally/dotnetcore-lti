using System.Security.Claims;
using System.Text.Json;

namespace Tpcly.Lti.Extensions;

public static class ClaimExtensions
{
    public static Claim? Get(this IEnumerable<Claim> claims, string type)
    {
        return claims.FirstOrDefault(x => x.Type == type);
    }

    public static string? GetValue(this IEnumerable<Claim> claims, string type)
    {
        return claims.Get(type)?.Value;
    }

    public static T? GetValue<T>(this IEnumerable<Claim> claims, string type, JsonSerializerOptions? options = null)
    {
        var value = claims.GetValue(type);
        return value == null
            ? default
            : JsonSerializer.Deserialize<T>(value, options);
    }

    public static IEnumerable<string> GetValues(this IEnumerable<Claim> claims, string type)
    {
        return claims.Where(c => c.Type == type).Select(static c => c.Value);
    }

    public static IEnumerable<T?> GetValues<T>(this IEnumerable<Claim> claims, string type, JsonSerializerOptions? options = null)
    {
        var values = claims.GetValues(type);
        return values.Select(v => JsonSerializer.Deserialize<T>(v, options));
    }
}