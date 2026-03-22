using Microsoft.EntityFrameworkCore;

namespace Gifteee;

internal sealed class WeatherContext : DbContext
{
    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> Forecasts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().HasKey(x => x.Date);
        base.OnModelCreating(modelBuilder);
    }
}