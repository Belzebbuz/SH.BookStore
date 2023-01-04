using Application.Contracts.Services.Identity;

using Mapster;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using SH.Bookstore.Identity.Infrastructure.Context.Settings;
using SH.Bookstore.Identity.Infrastructure.DTOs;
using SH.Bookstore.Identity.Infrastructure.Identity;
using SH.Bookstore.Identity.Infrastructure.Identity.Entities;
using SH.Bookstore.Identity.Infrastructure.Services.Messages;
using SH.Bookstore.Shared.Authentication;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Shared.Wrapper;
using Throw;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Identity.Services;

internal class UserService : IUserService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IPaginationService _paginationService;
	private readonly SecuritySettings _securitySettings;

	public UserService(
		UserManager<AppUser> userManager,
		RoleManager<IdentityRole> roleManager,
		IOptions<SecuritySettings> securityOptions,
		IPaginationService paginationService)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_paginationService = paginationService;
		_securitySettings = securityOptions.Value;
	}
	public async Task<IResult> AssignRolesAsync(AssignRolesRequest request)
	{
		var user = await _userManager.FindByIdAsync(request.UserId);
        user.ThrowIfNull("User not found!");

		if (user.Email == _securitySettings.RootUserEmail
			&& request.Roles.Any(a => !a.Enabled && a.RoleName == SHRoles.Admin))
		{
			await Result.FailAsync("Can't remove admin role from root use");
		}

		foreach (var userRole in request.Roles)
		{
			if (await _roleManager.FindByNameAsync(userRole.RoleName) is not null)
			{
				if (userRole.Enabled)
				{
					if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
					{
						await _userManager.AddToRoleAsync(user, userRole.RoleName);
					}
				}
				else
				{
					await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
				}
			}
		}
		return await Result.SuccessAsync();
	}

	public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
        user.ThrowIfNull("User not found!");

		var identityResult = await _userManager.ChangePasswordAsync(
			user,
			request.OldPassword,
			request.NewPassword);
		var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
		return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
	}

	public async Task<IResult> CreateAsync(CreateUserRequest request, string origin)
	{
		var user = AppUser.Create(request.Email, request.UserName, request.FirstName, request.LastName, request.PhoneNumber);
		var result = await _userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
			return await Result.FailAsync(messages: result.Errors.Select(x => x.Description).ToList());

		await _userManager.AddToRoleAsync(user, SHRoles.Basic);
		return await Result.SuccessAsync(user.Id);
	}

	public async Task<IResult> DeleteAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
        user.ThrowIfNull("User not found!");
		await _userManager.DeleteAsync(user);
		return Result.Success();
	}

	public async Task<IResult<UserDto>> GetAsync(string id)
	{
		var user = await _userManager.Users
			.AsNoTracking()
			.SingleOrDefaultAsync(u => u.Id == id);

        user.ThrowIfNull("User not found!");

		return await Result<UserDto>.SuccessAsync(data: user.Adapt<UserDto>());
	}

	public async Task<PaginatedResult<UserDto>> GetListAsync()
	{
		var count = await _userManager.Users.CountAsync();
		var users = await _userManager.Users
			.AsNoTracking()
			.OrderBy(x => x.Email)
            .Skip((_paginationService.Page - 1) * _paginationService.ItemsPerPage)
            .Take(_paginationService.ItemsPerPage)
			.ToListAsync();
		return PaginatedResult<UserDto>.Success(users.Adapt<List<UserDto>>(), count, _paginationService.Page, _paginationService.ItemsPerPage);
	}

	public async Task<PaginatedResult<UserDto>> GetListByFilterAsync(SearchFilterRequest request)
	{
		var usersRequest = _userManager.Users
			.AsNoTracking();
		if (!string.IsNullOrEmpty(request.Email))
		{
			usersRequest = usersRequest.Where(x => x.Email.ToLower().Contains(request.Email.ToLower()));
		}
        var count = await _userManager.Users.CountAsync();
		var users = await usersRequest
            .OrderBy(x => x.Email)
            .Skip((_paginationService.Page - 1) * _paginationService.ItemsPerPage)
            .Take(_paginationService.ItemsPerPage)
            .ToListAsync();
		return PaginatedResult<UserDto>.Success(users.Adapt<List<UserDto>>(),
                count,
                _paginationService.Page,
                _paginationService.ItemsPerPage);
	}

	public async Task<IResult<List<UserRoleDto>>> GetRolesAsync(string id)
	{
		var userRoles = new List<UserRoleDto>();

		var user = await _userManager.FindByIdAsync(id);
		var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
		foreach (var role in roles)
		{
			userRoles.Add(new UserRoleDto
			{
				RoleId = role.Id,
				RoleName = role.Name,
				Enabled = await _userManager.IsInRoleAsync(user, role.Name)
			});
		}

		return await Result<List<UserRoleDto>>.SuccessAsync(data: userRoles);
	}

	public async Task<IResult> ToggleStatusAsync(ToggleUserStatusRequest request)
	{
		var user = await _userManager.Users
			.SingleOrDefaultAsync(u => u.Id == request.UserId);
        user.ThrowIfNull("User not found!");
		bool isAdmin = await _userManager.IsInRoleAsync(user, SHRoles.Admin);
		if (isAdmin && !request.IsActive)
			return await Result.FailAsync("Can't deactivate admin user");

		user.ToggleStatus(request.IsActive);
		await _userManager.UpdateAsync(user);
		return await Result.SuccessAsync();
	}
}
