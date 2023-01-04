namespace SH.Bookstore.Identity.Infrastructure.DTOs;
public class UserRoleDto
{
    public UserRoleDto()
    {
    }
    public UserRoleDto(string? roleId, string? roleName, bool enabled)
    {
        RoleId = roleId;
        RoleName = roleName;
        Enabled = enabled;
    }

    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public bool Enabled { get; set; }
}
