using SH.Bookstore.Identity.Infrastructure.DTOs;
using SH.Bookstore.Identity.Infrastructure.Services.Messages;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Wrapper;

namespace Application.Contracts.Services.Identity;

public interface IUserService : IScopedService
{
    Task<IResult> AssignRolesAsync(AssignRolesRequest request);
    Task<IResult> CreateAsync(CreateUserRequest request, string origin);
    Task<IResult<UserDto>> GetAsync(string id);
    Task<PaginatedResult<UserDto>> GetListAsync();
    Task<IResult<List<UserRoleDto>>> GetRolesAsync(string id);
    Task<IResult> ToggleStatusAsync(ToggleUserStatusRequest request);
    Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, string userId);
    Task<PaginatedResult<UserDto>> GetListByFilterAsync(SearchFilterRequest request);
    Task<IResult> DeleteAsync(string id);
}
