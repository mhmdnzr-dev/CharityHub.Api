using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Presentation;


public static class DependencyInjection
{

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            // تنظیم نسخه‌های API
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"API Version {description.GroupName.ToUpper()}",
                    Version = description.ApiVersion.ToString(),
                    Description = $"API Documentation for version {description.ApiVersion}"
                });
            }

            // حل تداخل‌ها
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.DocInclusionPredicate((version, desc) => desc.GroupName == version);

            // پیکربندی احراز هویت Bearer Token
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsIn...\""
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        });
    }

    public static void AddVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true; // در صورت مشخص نشدن نسخه، از نسخه پیش‌فرض استفاده می‌کند.
            options.DefaultApiVersion = new ApiVersion(1, 0);  // نسخه پیش‌فرض
            options.ReportApiVersions = true; // گزارش نسخه‌های پشتیبانی‌شده
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; // قالب‌بندی نسخه‌ها (مثلاً v1, v2)
            options.SubstituteApiVersionInUrl = true; // جایگذاری نسخه در URL
        });
    }
    public static void AddCORSPolicy(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        var allowedDomains = configuration.GetSection("AllowedOrigins").Get<string[]>();

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                // The allowed origins will be resolved at runtime
                policy.WithOrigins(allowedDomains)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
    }
}
