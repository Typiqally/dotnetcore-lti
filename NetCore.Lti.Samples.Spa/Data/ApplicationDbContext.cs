using Microsoft.EntityFrameworkCore;
using NetCore.Lti.EntityFrameworkCore;

namespace NetCore.Lti.Samples.Spa.Data;
 
public class ApplicationDbContext : LtiDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ToolPlatform>().HasData(Constants.CanvasDockerToolPlatform);
    }
}