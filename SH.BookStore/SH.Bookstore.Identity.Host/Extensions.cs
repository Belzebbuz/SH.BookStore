namespace SH.Bookstore.Identity.Host;
internal static class Extensions
{
    internal static ConfigureHostBuilder AddConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            const string configurationsDirectory = "Configurations";
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/serilog.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        });

        return host;
    }
}
