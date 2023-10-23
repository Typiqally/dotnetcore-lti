using Microsoft.IdentityModel.Tokens;
using Tpcly.Persistence.Abstractions;

namespace Tpcly.Lti;

public class ToolPlatformService : IToolPlatformService
{
    private readonly HttpClient _http;
    private readonly IRepository<ToolPlatform> _tenantRepository;

    public ToolPlatformService(HttpClient http, IRepository<ToolPlatform> tenantRepository)
    {
        _http = http;
        _tenantRepository = tenantRepository;
    }

    public async Task<ToolPlatform?> GetById(string id)
    {
        return await _tenantRepository.SingleOrDefaultAsync(platform => platform.Id == id);
    }

    public async Task<JsonWebKeySet?> GetJwks(string id)
    {
        var toolPlatform = await _tenantRepository.SingleOrDefaultAsync(platform => platform.Id == id);
        return toolPlatform != null ? await GetJwks(toolPlatform) : null;
    }

    public async Task<JsonWebKeySet?> GetJwks(ToolPlatform toolPlatform)
    {
        var response = await _http.GetAsync(toolPlatform.JwkSetUrl);
        return new JsonWebKeySet(await response.Content.ReadAsStringAsync());
    }
}