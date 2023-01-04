using Microsoft.Extensions.Hosting.WindowsServices;

using Serilog;

using SH.Bookstore.Books.Application;
using SH.Bookstore.Books.Host;
using SH.Bookstore.Books.Infrastructure;
using SH.Bookstore.Shared.Common.Services;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
if (WindowsServiceHelpers.IsWindowsService())
    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
try
{
    var options = new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
    };
    var builder = WebApplication.CreateBuilder(options);
    builder.Host.AddConfigurations();
    builder.Host.UseWindowsService();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    await app.UseInfrastructureAsync(app.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
