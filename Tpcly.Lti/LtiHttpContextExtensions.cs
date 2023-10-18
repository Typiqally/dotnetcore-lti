using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Tpcly.Lti;

public static class LtiHttpContextExtensions
{
    public static async Task<LtiMessage?> GetLtiMessageAsync(this HttpContext context, string tokenName = "access_token")
    {
        var token = await context.GetTokenAsync(tokenName);
        return token != null ? new LtiMessage(token) : null;
    }
}