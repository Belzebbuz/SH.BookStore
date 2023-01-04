using Microsoft.Extensions.Hosting.WindowsServices;

if (WindowsServiceHelpers.IsWindowsService())
    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
};
var builder = WebApplication.CreateBuilder(options);

builder.Host.UseWindowsService();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("yarp"));
builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", policy => policy
                                                                .AllowAnyHeader()
                                                                .AllowAnyOrigin()
                                                                .AllowAnyMethod()));
var app = builder.Build();

app.MapGet("/", () => "Gateway");
app.UseRouting();
app.UseCors("CorsPolicy");
app.MapReverseProxy();
app.Run();