namespace CharityHub.Presentation.Middleware;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

internal sealed class StaticFileMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _staticFilePath;
    private readonly string _requestPath;

    public StaticFileMiddleware(RequestDelegate next, IOptions<StaticFileMiddlewareOptions> options)
    {
        _next = next;
        _staticFilePath = options.Value.StaticFilePath;
        _requestPath = options.Value.RequestPath;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments(_requestPath, StringComparison.OrdinalIgnoreCase))
        {
            var filePath = Path.Combine(_staticFilePath, context.Request.Path.Value.Substring(_requestPath.Length).TrimStart('/'));

            if (File.Exists(filePath))
            {
                context.Response.ContentType = GetContentType(filePath);
                await context.Response.SendFileAsync(filePath);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("File not found.");
            return;
        }

        await _next(context);
    }

    private string GetContentType(string filePath)
    {
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        return provider.TryGetContentType(filePath, out var contentType) ? contentType : "application/octet-stream";
    }
}


public class StaticFileMiddlewareOptions
{
    public required string StaticFilePath { get; set; }
    public required string RequestPath { get; set; }
}

