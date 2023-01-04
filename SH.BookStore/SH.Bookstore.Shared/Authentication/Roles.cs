using System.Collections.ObjectModel;

namespace SH.Bookstore.Shared.Authentication;
public class SHRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);
    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Basic
    });
}