namespace Gifteee;

using Microsoft.EntityFrameworkCore;

internal sealed class DbInitializer
{
    private readonly WeatherContext _weatherContext;

    public DbInitializer(WeatherContext weatherContext)
    {
        _weatherContext = weatherContext;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _weatherContext.Database.EnsureCreatedAsync(cancellationToken);

        if (await _weatherContext.Forecasts.AnyAsync<WeatherForecast>(cancellationToken))
            return;

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        var forecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ));
        _weatherContext.Forecasts.AddRange(forecasts);
        await _weatherContext.SaveChangesAsync(cancellationToken);
    }
}