namespace SH.Bookstore.Identity.Infrastructure.Context.Settings;

public class JwtSettings
{
    public string Key { get; set; }
    public int ExpirationInDays { get; set; }
}
