using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CharityHub.Presentation.Extensions;



internal static class HttpContextExtensions
{
    public static bool IsDevelopment(this HttpContext context)
    {
        var environment = context.RequestServices.GetService<IWebHostEnvironment>();
        return environment?.IsDevelopment() ?? false;
    }
}