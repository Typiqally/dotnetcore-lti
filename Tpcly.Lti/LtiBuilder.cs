using Microsoft.Extensions.DependencyInjection;
using NetCore.Persistence.Memory;

namespace Tpcly.Lti;

public class LtiBuilder
{
    public LtiBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public LtiBuilder AddPlatforms(IEnumerable<ToolPlatform> platforms)
    {
        Services.AddMemoryBasedRepository<ToolPlatform>(options =>
        {
            options.Data = platforms.ToDictionary(static e => e.Id as object, static e => e);
        });

        return this;
    }
}