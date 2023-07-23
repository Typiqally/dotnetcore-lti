using Microsoft.EntityFrameworkCore;

namespace Tpcly.Lti.EntityFrameworkCore;

public interface ILtiDbContext
{
    public DbSet<ToolPlatform> ToolPlatforms { get; set; }
}