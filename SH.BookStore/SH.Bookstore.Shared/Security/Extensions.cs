using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using System.Text;

namespace SH.Bookstore.Shared.Security;
public static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, string jwtKey)
    {
        byte[] key = Encoding.ASCII.GetBytes(jwtKey);
        services.AddAuthentication(authentication =>
        {
            authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearer =>
        {
            bearer.RequireHttpsMetadata = false;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            bearer.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                        throw new Exception("Authentication Failed.");

                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new Exception("You are not authorized to access this resource."),
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    if (!string.IsNullOrEmpty(accessToken) &&
                        context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                        // Read the token out of the query string
                        context.Token = accessToken;

                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }
}
