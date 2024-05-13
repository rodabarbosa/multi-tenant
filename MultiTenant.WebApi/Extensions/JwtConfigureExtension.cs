using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Application.Models;
using MultiTenant.Application.Utils;
using System.Text;

namespace MultiTenant.WebApi.Extensions;

/// <summary>
/// ServiceCollection Extensions
/// </summary>
static public class JwtConfigureExtension
{
    /// <summary>
    /// Add JWT configuration to services
    /// </summary>
    /// <param name="builder"></param>
    public static WebApplicationBuilder AddJwtService(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        return builder.AddAuthentication();
    }

    private static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration
            .GetSection("Jwt");

        builder.Services.Configure<JwtOption>(config);

        var jwtOptions = config.Get<JwtOption>();

        builder.Services.AddSingleton<TokenUtil>();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Events.OnRedirectToAccessDenied =
                    options.Events.OnRedirectToLogin = c =>
                    {
                        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.FromResult<object>(null!);
                    };
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                var paramsValidation = options.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));
                paramsValidation.ValidAudience = jwtOptions.Audience;
                paramsValidation.ValidIssuer = jwtOptions.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

        _ = builder.Services.AddAuthorization(options =>
        {
            options.InvokeHandlersAfterFailure = true;
            options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
        });

        return builder;
    }
}
