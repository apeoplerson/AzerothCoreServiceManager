





using AzerothCoreManager.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/manager.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

try
{
    Log.Information("Starting AzerothCoreManager.Service");

    var builder = WebApplication.CreateBuilder(args);

    // Configure services
    builder.Services.AddHostedService<Worker>();
    builder.Services.AddSingleton<IProcessManager, ProcessManager>();

    // Add configuration
    builder.Services.Configure<AzerothCorePaths>(builder.Configuration.GetSection("AzerothCorePaths"));
    builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("Security"));

    // Add Minimal APIs
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure HTTP pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();

    // Define minimal API endpoints with authentication
    app.MapGet("/health", () => Results.Ok("Healthy"));

    app.MapGet("/status", (IProcessManager processManager) =>
    {
        var status = new
        {
            auth = new { running = processManager.IsAuthServerRunning(), pid = processManager.GetAuthServerPid() },
            world = new { running = processManager.IsWorldServerRunning(), pid = processManager.GetWorldServerPid() }
        };
        return Results.Ok(status);
    });

    app.MapPost("/auth/start", async (IProcessManager processManager) =>
    {
        await processManager.StartAuthServer();
        return Results.Ok();
    });

    app.MapPost("/auth/stop", async (IProcessManager processManager) =>
    {
        await processManager.StopAuthServer();
        return Results.Ok();
    });

    app.MapPost("/world/start", async (IProcessManager processManager) =>
    {
        await processManager.StartWorldServer();
        return Results.Ok();
    });

    app.MapPost("/world/stop", async (IProcessManager processManager) =>
    {
        await processManager.StopWorldServer();
        return Results.Ok();
    });

    // Serve static files for UI
    app.UseStaticFiles();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}





