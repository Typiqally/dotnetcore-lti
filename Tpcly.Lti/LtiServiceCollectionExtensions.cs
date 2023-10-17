using Microsoft.Extensions.DependencyInjection;

namespace Tpcly.Lti;

public static class LtiServiceCollectionExtensions
{
    public static LtiBuilder AddLti(this IServiceCollection services)
    {
        services.AddHttpClient<IToolPlatformService, ToolPlatformService>();
        services.AddScoped<ILtiTokenValidator, LtiTokenValidator>();

        return new LtiBuilder(services);
    }
}