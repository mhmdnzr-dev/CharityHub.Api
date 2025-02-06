using System.Text.Json;

using Microsoft.AspNetCore.Http;

namespace CharityHub.Presentation.Middleware;

using Filters;

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

internal sealed class PagedDataResponseMiddleware
{
    private readonly RequestDelegate _next;

    public PagedDataResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;

            await _next(context);

            // Only process JSON responses
            if (context.Response.ContentType?.Contains("application/json") == true)
            {
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                responseBodyStream.Seek(0, SeekOrigin.Begin);

                bool isEmptyResponse = false;

                try
                {
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(responseBody);

                    // Check if `data.items` is empty
                    if (jsonElement.TryGetProperty("data", out var dataElement) &&
                        dataElement.TryGetProperty("items", out var itemsElement))
                    {
                        isEmptyResponse = itemsElement.ValueKind == JsonValueKind.Null ||
                                          (itemsElement.ValueKind == JsonValueKind.Array && itemsElement.GetArrayLength() == 0);
                    }
                }
                catch
                {
                    // Ignore errors, assume valid JSON
                }

                if (isEmptyResponse)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    responseBodyStream.SetLength(0); // Clear response body
                    await context.Response.WriteAsync("Not found!"); // Minimal response
                    return;
                }
            }

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }
}
