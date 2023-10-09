using Microsoft.EntityFrameworkCore;

namespace Tpcly.Lti.EntityFrameworkCore;

public interface ILtiDbContext
{
    public DbSet<ToolPlatform> ToolPlatforms { get; set; }
    
    public DbSet<LaunchSession> LaunchSessions { get; set; }
}