using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MultiTenant.Application.Models;
using MultiTenant.WebApi.Filters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace MultiTenant.WebApi.Extensions;

/// <summary>
/// ServiceCollection Extensions
/// </summary>
static public class SwaggerExtension
{
    /// <summary>
    ///  Add swagger configuration to services.
    /// </summary>
    /// <param name="builder"></param>
    static public WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {
        var apiDescription = new ApiDescriptionConfig();
        new ConfigureFromConfigurationOptions<ApiDescriptionConfig>(builder.Configuration.GetSection("ApiDescription"))
            .Configure(apiDescription);

        builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

        builder.Services.AddSwaggerGen(options =>
        {
            var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = !string.IsNullOrEmpty(apiDescription.Title) ? $"{apiDescription.Title} {description.ApiVersion}" : default,
                    Version = description.ApiVersion.ToString(),
                    Description = !string.IsNullOrEmpty(apiDescription.Description) ? apiDescription.Description : default,
                    TermsOfService = !string.IsNullOrEmpty(apiDescription.TermOfService) ? new Uri(apiDescription.TermOfService) : default,
                    Contact = new OpenApiContact
                    {
                        Email = !string.IsNullOrEmpty(apiDescription.ContactEmail) ? apiDescription.ContactEmail : default,
                        Name = !string.IsNullOrEmpty(apiDescription.ContactName) ? apiDescription.ContactName : default,
                        Url = !string.IsNullOrEmpty(apiDescription.ContactSite) ? new Uri(apiDescription.ContactSite) : default
                    },
                    License = default
                });
            }

            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.OperationFilter<SwaggerFilter>();

            var assemblyName = Assembly.GetEntryAssembly()!
                .GetName()
                .Name;

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml"));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Description = "Authorization token using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Type = SecuritySchemeType.Http
            });
        });

        return builder;
    }

    /// <summary>
    /// Use swagger
    /// </summary>
    /// <param name="app"></param>
    public static WebApplication UseSwaggerApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.DocExpansion(DocExpansion.None); });

        return app;
    }
}
