using CharityHub.Endpoints;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Data.SeedData;
using CharityHub.Infra.Sql.Extensions;
using CharityHub.Presentation;
using CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddSeeder<DatabaseSeeder>();

var uploadDirectory = builder.Configuration["FileSettings:UploadDirectory"] ?? "uploads";
uploadDirectory = uploadDirectory.TrimStart('/');

var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory);

builder.Services.Configure<StaticFileMiddlewareOptions>(options =>
{
    options.StaticFilePath = staticFilesPath;
    options.RequestPath = $"/{uploadDirectory}";
});


var app = builder.Build();

// Apply migrations and seed data
await app.Services.SeedAsync<CharityHubCommandDbContext>();

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

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseOutputCache();
app.UseRouting();
app.UseAuthentication();



if (!Directory.Exists(staticFilesPath))
{
    Directory.CreateDirectory(staticFilesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesPath),
    RequestPath = $"/{uploadDirectory}", // Ensure URL path is correct
    ServeUnknownFileTypes = true, 
    DefaultContentType = "application/octet-stream"
});

// 🔥 Ensure middleware receives the correct static file path
app.UseStaticFileResponseMiddleware();

app.UseAuthorization();
app.UsePagedDataResponseMiddleware();
app.UseBaseResponseMiddleware();
app.UseExceptionResponseMiddleware();
app.MapControllers();
app.Run();