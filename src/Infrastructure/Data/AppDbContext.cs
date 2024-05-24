using ApplicationCore.Entities.AnalyticsSettingsAggregate;
using ApplicationCore.Entities.VideoStreamAggregate;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<VideoStream> Stream => Set<VideoStream>();
    public DbSet<AnalyticsSettings> AnalyticsSettings => Set<AnalyticsSettings>();
}
