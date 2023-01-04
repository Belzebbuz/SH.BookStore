﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSwag;
using NSwag.Generation.Processors.Security;

namespace SH.Bookstore.Shared.OpenApi;

public static class Extensions
{
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings.UseSwagger)
        {
            services.AddEndpointsApiExplorer();
            services.AddOpenApiDocument((document, serviceProvider) =>
            {
                document.PostProcess = doc =>
                {
                    doc.Info.Title = settings.Title;
                    doc.Info.Contact = new() { Email = settings.ContactEmail };
                };
                document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Input your Bearer token to access this API",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                });
                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                document.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());
            });
        }

        return services;
    }

    public static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings.UseSwagger)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
        return app;
    }
}
