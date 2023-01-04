namespace SH.Bookstore.Identity.Infrastructure.Context.Settings;
public class ServicesSeedSettings
{
    public List<Credentials> Services { get; set; }
}

public class Credentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}