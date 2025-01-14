using CharityHub.Endpoints;
using CharityHub.Presentation.Extensions;
using CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCustomServices();


var app = builder.Build();

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
app.UseMiddleware<BaseResponseMiddleware>();
app.MapControllers();
app.Run();
