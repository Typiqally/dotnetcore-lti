using Microsoft.Extensions.DependencyInjection;
using Tpcly.Persistence.Memory;

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

    public LtiBuilder AddToolProvider(Action<LtiToolProviderOptions> options)
    {
        Services.Configure(options);
        Services.AddScoped<ILaunchSessionService, LaunchSessionService>();

        return this;
    }
}