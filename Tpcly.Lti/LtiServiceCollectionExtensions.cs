using Microsoft.Extensions.DependencyInjection;

namespace Tpcly.Lti;

public static class LtiServiceCollectionExtensions
{
    public static LtiBuilder AddLti(this IServiceCollection services, Action<LtiOptions> options)
    {
        services.AddHttpClient<IToolPlatformService, ToolPlatformService>();
        services.AddScoped<ILtiTokenValidator, LtiTokenValidator>();

        services.Configure(options);

        return new LtiBuilder(services);
    }
}