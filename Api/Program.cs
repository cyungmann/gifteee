using System.Collections;
using Gifteee;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// Add services to the container.

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContextFactory<WeatherContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("gifteee"), x => x.EnableRetryOnFailure()));
builder.Services.AddTransient<DbInitializer>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders();
    app.MapOpenApi();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/weatherforecast", async ([FromServices] IDbContextFactory<WeatherContext> dbContextFactory, CancellationToken cancellationToken) =>
{
    await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
    return await dbContext.Forecasts.ToListAsync(cancellationToken);
})
.WithName("GetWeatherForecast");

app.MapFallbackToFile("/index.html");

LogDebugInfo(app.Logger);

await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbInitializer = services.GetRequiredService<DbInitializer>();
        await dbInitializer.InitializeAsync(CancellationToken.None);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the database.");
    }
}

app.Run();

void LogDebugInfo(ILogger logger)
{
    var envVars = Environment.GetEnvironmentVariables();
    Console.WriteLine("Environment variables:");
    var envVarsList = new List<string>();
    foreach (DictionaryEntry entry in envVars)
    {
        Console.WriteLine($"  {entry.Key}={entry.Value}");
        envVarsList.Add($"{entry.Key}={entry.Value}");
    }
    logger.LogInformation("Environment variables: {EnvironmentVariables}", string.Join(", ", envVarsList));

    Console.WriteLine("Configuration:");
    var configList = new List<string>();
    foreach (var (key, value) in app.Configuration.AsEnumerable())
    {
        Console.WriteLine($"  {key}={value}");
        configList.Add($"{key}={value}");
    }
    logger.LogInformation("Configuration: {Configuration}", string.Join(", ", configList));
}