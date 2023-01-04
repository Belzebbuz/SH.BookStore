using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SH.Bookstore.Identity.Infrastructure.Context.Settings;
using SH.Bookstore.Identity.Infrastructure.Identity.Entities;
using SH.Bookstore.Shared.Authentication;

namespace SH.Bookstore.Identity.Infrastructure.Context;
public class DbSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DbSeeder> _logger;
    private readonly SecuritySettings _securitySettings;
    private readonly ServicesSeedSettings _servicesSettings;

    public DbSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
        IOptions<SecuritySettings> securitySettings,
        ILogger<DbSeeder> logger, IOptions<ServicesSeedSettings> servicesOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _securitySettings = securitySettings.Value;
        _servicesSettings = servicesOptions.Value;
    }

    public async Task SeedDataAsync()
    {
        foreach (var role in SHRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(x => x.Name == role) == null)
            {
                await _roleManager.CreateAsync(new()
                {
                    Name = role
                });
                _logger.LogInformation($"Role {role} created");
            }
        }
        if (await _userManager.Users.SingleOrDefaultAsync(x => x.Email == _securitySettings.RootUserEmail) == null)
        {
            var adminUser = AppUser.Create(
                _securitySettings.RootUserEmail,
                _securitySettings.RootUserEmail,
                _securitySettings.RootUserEmail,
                _securitySettings.RootUserEmail, null);
            await _userManager.CreateAsync(adminUser, _securitySettings.DefaultPassword);
            await _userManager.AddToRoleAsync(adminUser, SHRoles.Admin);
            await _userManager.AddToRoleAsync(adminUser, SHRoles.Basic);
            _logger.LogInformation($"Root user {adminUser.Email} created");
        }

        foreach (var serviceSettings in _servicesSettings.Services)
        {
            if(await _userManager.FindByEmailAsync(serviceSettings.Email) == null)
            {
                var serviceUser = AppUser.Create(
                    serviceSettings.Email,
                    serviceSettings.Email,
                    serviceSettings.Email,
                    serviceSettings.Email,null);
                await _userManager.CreateAsync(serviceUser, _securitySettings.DefaultPassword);
                await _userManager.AddToRoleAsync(serviceUser, SHRoles.Admin);
                await _userManager.AddToRoleAsync(serviceUser, SHRoles.Basic);
                _logger.LogInformation($"Serivice user {serviceUser.Email} created");
            }
        }
    }
}
