using MultiTenant.Application.Handlers.Authenticate;
using MultiTenant.Application.Handlers.Persons;
using MultiTenant.Application.Handlers.Tenants;
using MultiTenant.Application.Handlers.Users;
using MultiTenant.Domain.Interfaces;

namespace MultiTenant.WebApi.Extensions;

public static class ServiceProviderExtension
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IAuthenticationHandler, AuthenticationHandler>()
            .AddScoped<ITenantPostHandler, TenantPostHandler>()
            .AddScoped<ITenantGetAllHandler, TenantGetAllHandler>()
            .AddScoped<ITenantMigrateHandler, TenantMigrateHandler>()
            .AddScoped<IUserPostHandler, UserPostHandler>()
            .AddScoped<IUserGetAllHandler, UserGetAllHandler>()
            .AddScoped<IPersonGetAllHandler, PersonGetAllHandler>();

        return builder;
    }
}
