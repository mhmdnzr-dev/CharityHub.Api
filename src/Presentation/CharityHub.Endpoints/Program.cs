namespace CharityHub.Endpoints;

using System.Threading.RateLimiting;

using Core.Contract.Primitives.Models;

using Infra.Sql.Data.DbContexts;
using Infra.Sql.Data.SeedData;
using Infra.Sql.Extensions;

using Microsoft.AspNetCore.DataProtection;

using Presentation;
using Presentation.Middleware;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;

using Serilog;



public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/home/app/.aspnet/DataProtection-Keys"))
            .SetApplicationName("CharityHubApi");
        
        // TODO: uncomment on production to prevent DDOS attacks
        // builder.Services.AddRateLimiter(options =>
        // {
        //     options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        //         httpContext => RateLimitPartition.GetFixedWindowLimiter(
        //             partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "global",
        //             factory: _ => new FixedWindowRateLimiterOptions
        //             {
        //                 PermitLimit = 5, 
        //                 Window = TimeSpan.FromSeconds(30), 
        //                 QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        //                 QueueLimit = 0
        //             }));
        //
        //     options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        // });
        
        builder.Services.AddHttpClient();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog();

        builder.Services.AddMemoryCache();
        builder.Services.AddCustomServices(builder.Configuration);
        builder.Services.AddSeeder<DatabaseSeeder>();

        var uploadDirectory = builder.Configuration["FileSettings:UploadDirectory"] ?? "uploads";
        uploadDirectory = uploadDirectory.TrimStart('/');

        var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory);

        builder.Services.Configure<StaticFileDto>(options =>
        {
            options.StaticFilePath = staticFilesPath;
            options.RequestPath = $"/{uploadDirectory}";
        });

        var app = builder.Build();

        await ApplyMigrationsAndSeedData(app);

        ConfigureSwagger(app);

        ConfigureMiddleware(app, staticFilesPath, uploadDirectory);

        app.MapControllers();

        await app.RunAsync();
    }

    private static async Task ApplyMigrationsAndSeedData(WebApplication app)
    {
        await app.Services.SeedAsync<CharityHubCommandDbContext>();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        var isDevMode = app.Environment.IsDevelopment();
        if (isDevMode)
        {
            app.UseSwagger();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"API {description.GroupName.ToUpper()}");
                }
            });
        }
    }

    private static void ConfigureMiddleware(WebApplication app, string staticFilesPath, string uploadDirectory)
    {
        // TODO: uncomment on production to prevent DDOS attacks
        // app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseOutputCache();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        if (!Directory.Exists(staticFilesPath))
        {
            Directory.CreateDirectory(staticFilesPath);
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = $"/{uploadDirectory}",
            ServeUnknownFileTypes = true,
            DefaultContentType = "application/octet-stream"
        });
        
        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Request from IP: {context.Connection.RemoteIpAddress}, Path: {context.Request.Path}");
            await next();
        });

 
 
        
        app.UseBaseResponseMiddleware();
     
    }
}