using Microsoft.Extensions.DependencyInjection;
using NetCore.Lti.Data;

namespace NetCore.Lti;

public class LtiBuilder
{
    public LtiBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public LtiBuilder AddPlatforms(IEnumerable<ToolPlatform> platforms)
    {
        Services.AddScoped<IRepository<ToolPlatform>, MemoryBasedRepository<ToolPlatform>>(_ => new MemoryBasedRepository<ToolPlatform>(platforms));

        return this;
    }
}