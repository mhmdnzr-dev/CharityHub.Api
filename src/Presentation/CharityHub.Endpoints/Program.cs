using CharityHub.Endpoints;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Data.SeedData;
using CharityHub.Infra.Sql.Extensions;
using CharityHub.Presentation;
using CharityHub.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add Custom Services
builder.Services.AddCustomServices();

// Register Seeder
builder.Services.AddSeeder<DatabaseSeeder>();

var app = builder.Build();

// Apply migrations and seed data
await app.Services.SeedAsync<CharityHubCommandDbContext>();

var isDevMode = app.Environment.IsDevelopment();

if (true)
{
    app.UseSwagger();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"API {description.GroupName.ToUpper()}");
        }
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseOutputCache();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseBaseResponseMiddleware();
app.UseExceptionResponseMiddleware();
app.MapControllers();
app.Run();