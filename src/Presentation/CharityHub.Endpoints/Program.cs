
using CharityHub.Endpoints.DependencyInjection;
using CharityHub.Presentation.Extensions;
using CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // در صورت مشخص نشدن نسخه، از نسخه پیش‌فرض استفاده می‌کند.
    options.DefaultApiVersion = new ApiVersion(1, 0);  // نسخه پیش‌فرض
    options.ReportApiVersions = true; // گزارش نسخه‌های پشتیبانی‌شده
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // قالب‌بندی نسخه‌ها (مثلاً v1, v2)
    options.SubstituteApiVersionInUrl = true; // جایگذاری نسخه در URL
});



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCORSPolicy();

builder.Services.AddIdentityAuthorization();
builder.Services.AddCustomServices();
builder.Services.AddDbContext();

// Learn more about configuring OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = $"api version {description.GroupName.ToUpper()}",
            Version = description.ApiVersion.ToString(),
            Description = $"API Documentation for version {description.ApiVersion}"
        });
    }

    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.DocInclusionPredicate((version, desc) =>
    {
        return desc.GroupName == version;
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BaseResponseMiddleware>();
app.MapControllers();
app.Run();
