using Microsoft.IdentityModel.Tokens;

namespace Tpcly.Lti;

public interface IToolPlatformService
{
    Task<ToolPlatform?> GetById(string id);

    Task<JsonWebKeySet?> GetJwks(string id);

    Task<JsonWebKeySet?> GetJwks(ToolPlatform toolPlatform);
}