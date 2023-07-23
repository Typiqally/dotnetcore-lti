using Microsoft.EntityFrameworkCore;
using NetCore.Persistence.EntityFrameworkCore;

namespace Tpcly.Lti.EntityFrameworkCore;

public static class EntityFrameworkCoreLtiBuilderExtensions
{
    public static LtiBuilder AddEntityFrameworkRepositories<TContext>(this LtiBuilder builder)
        where TContext : DbContext, ILtiDbContext
    {
        builder.Services.AddEntityFrameworkRepository<TContext, ToolPlatform>();

        return builder;
    }
}