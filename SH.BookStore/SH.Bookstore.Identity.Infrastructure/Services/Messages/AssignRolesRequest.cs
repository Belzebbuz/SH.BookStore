using SH.Bookstore.Identity.Infrastructure.DTOs;

namespace SH.Bookstore.Identity.Infrastructure.Services.Messages;
public record AssignRolesRequest(string UserId, List<UserRoleDto> Roles);
