using Microsoft.EntityFrameworkCore;
using Tpcly.Lti;
using Tpcly.Lti.EntityFrameworkCore;

namespace Tpcly.Lti.Samples.Spa.Data;
 
public class ApplicationDbContext : DbContext, ILtiDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ToolPlatform> ToolPlatforms { get; set; }
}