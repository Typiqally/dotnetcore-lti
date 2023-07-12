using Microsoft.EntityFrameworkCore;

namespace NetCore.Lti.EntityFrameworkCore;

public interface ILtiDbContext
{
    public DbSet<ToolPlatform> ToolPlatforms { get; set; }
}