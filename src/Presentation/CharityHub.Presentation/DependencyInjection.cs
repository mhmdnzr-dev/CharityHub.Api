using CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Presentation;

using Filters;

public static class DependencyInjection
{
    public static void AddDotnetOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder =>
                builder.Expire(TimeSpan.FromSeconds(10)));
            options.AddPolicy("Expire20", builder =>
                builder.Expire(TimeSpan.FromSeconds(20)));
            options.AddPolicy("Expire30", builder =>
                builder.Expire(TimeSpan.FromSeconds(30)));
        });
    }

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

 
        options.OperationFilter<AuthorizeCheckOperationFilter>();

        // پیکربندی احراز هویت Bearer Token (If required)
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsIn...\""
        });

        // Apply security requirement (optional, for JWT authentication)
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
                new string[] {}
            }
        });
    });
}

    public static void AddVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified =
                true; // در صورت مشخص نشدن نسخه، از نسخه پیش‌فرض استفاده می‌کند.
            options.DefaultApiVersion = new ApiVersion(1, 0); // نسخه پیش‌فرض
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

    public static IApplicationBuilder UseExceptionResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionResponseMiddleware>();
    }


    public static IApplicationBuilder UseBaseResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BaseResponseMiddleware>();
    }
}