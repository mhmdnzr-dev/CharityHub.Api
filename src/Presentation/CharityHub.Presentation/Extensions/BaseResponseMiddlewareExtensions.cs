namespace CharityHub.Presentation.Extensions;

using CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Builder;

public static class BaseResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseBaseResponseMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BaseResponseMiddleware>();
    }
}
