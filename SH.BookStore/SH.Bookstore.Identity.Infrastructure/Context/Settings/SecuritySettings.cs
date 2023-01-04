namespace SH.Bookstore.Identity.Infrastructure.Context.Settings;
public class SecuritySettings
{
    public JwtSettings JwtSettings { get; set; }
    public string RootUserEmail { get; set; }
    public string DefaultPassword { get; set; }
    public bool RequireConfirmedAccount { get; set; }
}
