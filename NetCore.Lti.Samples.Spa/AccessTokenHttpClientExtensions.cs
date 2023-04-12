using IdentityModel.Client;

namespace NetCore.Lti.Samples.Spa;

public static class AccessTokenHttpClientExtensions
{
    public static IHttpClientBuilder AddAccessTokenManagement(this IHttpClientBuilder builder, string scheme)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AccessTokenDelegatingHandler>(provider =>
        {
            var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            var tokenClient = provider.GetRequiredService<TokenClient>();

            return new AccessTokenDelegatingHandler(scheme, httpContextAccessor, tokenClient);
        });

        return builder.AddHttpMessageHandler<AccessTokenDelegatingHandler>();
    }
}