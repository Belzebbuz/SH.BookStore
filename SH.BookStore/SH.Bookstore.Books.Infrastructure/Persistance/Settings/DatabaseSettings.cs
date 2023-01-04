namespace SH.Bookstore.Books.Infrastructure.Persistance.Settings;
internal class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public bool SeedTestData { get; set; }
}
