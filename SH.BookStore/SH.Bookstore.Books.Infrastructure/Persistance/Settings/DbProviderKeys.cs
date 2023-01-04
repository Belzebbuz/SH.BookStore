namespace SH.Bookstore.Books.Infrastructure.Persistance.Settings;

internal static class DbProviderKeys
{
    public const string SQLServer = nameof(SQLServer);
    public const string PostgreSQL = nameof(PostgreSQL);
    public const string Sqlite = nameof(Sqlite);
    public const string InMemory = nameof(InMemory);
}
