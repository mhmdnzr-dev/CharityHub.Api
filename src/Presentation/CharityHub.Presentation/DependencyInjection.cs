using CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Presentation;

using Filters;

using Microsoft.OpenApi.Models;

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

            // Set up API versions
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName,
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = $"API Version {description.GroupName.ToUpper()}",
                        Version = description.ApiVersion.ToString(),
                        Description = $"API Documentation for version {description.ApiVersion}"
                    });
            }

            // Resolve conflicts between different API versions
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.DocInclusionPredicate((version, desc) => desc.GroupName == version);

            // Define the Bearer authentication scheme in Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field"
            });




            options.OperationFilter<AuthorizeCheckOperationFilter>();

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

    public static IApplicationBuilder UsePagedDataResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PagedDataResponseMiddleware>();
    }


    public static IApplicationBuilder UseBaseResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BaseResponseMiddleware>();
    }
    
    public static IApplicationBuilder UseStaticFileResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StaticFileMiddleware>();
    }



    
}