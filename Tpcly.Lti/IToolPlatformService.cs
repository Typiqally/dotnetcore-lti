using IdentityModel.Jwk;

namespace Tpcly.Lti;

public interface IToolPlatformService
{
    Task<ToolPlatform?> GetById(string id);
    Task<JsonWebKeySet?> GetJwks(ToolPlatform tenant);
}