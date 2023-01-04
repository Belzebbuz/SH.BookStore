using Application.Contracts.Services.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SH.Bookstore.Identity.Infrastructure.Context.Settings;
using SH.Bookstore.Identity.Infrastructure.Identity.Entities;
using SH.Bookstore.Identity.Infrastructure.Services.Messages;
using SH.Bookstore.Shared.Wrapper;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Throw;

namespace Infrastructure.Identity.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SecuritySettings _securitySettings;
    public TokenService(
        UserManager<AppUser> userManager,
        IOptions<SecuritySettings> securitySettings)
    {
        _userManager = userManager;
        _securitySettings = securitySettings.Value;
    }
    public async Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());
        user
            .ThrowIfNull("User not found")
            .Throw("User not active")
            .IfFalse(x => x.IsActive);

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            return await Result<TokenResponse>.FailAsync("Account not confirmed!");
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return await Result<TokenResponse>.FailAsync("Email or password are wrong!");
        }

        return await Result<TokenResponse>.SuccessAsync(await GenerateTokensAndUpdateUser(user, ipAddress));
    }

    private async Task<TokenResponse> GenerateTokensAndUpdateUser(AppUser user, string ipAddress)
    {
        var roles = await _userManager.GetRolesAsync(user);
        string token = GenerateJwt(user, ipAddress, roles);

        await _userManager.UpdateAsync(user);

        return new TokenResponse(token);
    }

    private string GenerateJwt(AppUser user, string ipAddress, IList<string> roles) =>
       GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress, roles));

    private IEnumerable<Claim> GetClaims(AppUser user, string ipAddress, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }
        return claims;
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_securitySettings.JwtSettings.ExpirationInDays),
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_securitySettings.JwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        byte[] secret = Encoding.UTF8.GetBytes(_securitySettings.JwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}
