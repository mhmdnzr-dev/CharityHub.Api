using System.Text.Json;

using CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Http;

namespace CharityHub.Presentation.Middleware;
internal sealed class BaseResponseMiddleware
{
    private readonly RequestDelegate _next;

    public BaseResponseMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }



    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            if (!context.Items.ContainsKey("SkipBaseResponse"))
            {
                bool isEmptyResponse = false;
                object? parsedData = null;

                try
                {
                    if (!string.IsNullOrWhiteSpace(responseBody))
                    {
                        var jsonElement = JsonSerializer.Deserialize<JsonElement>(responseBody);

                        // Check if response is null or an empty array
                        isEmptyResponse = jsonElement.ValueKind == JsonValueKind.Null ||
                                          (jsonElement.ValueKind == JsonValueKind.Array && jsonElement.GetArrayLength() == 0);

                        // Check if response is a paged result and Items is null or empty
                        if (!isEmptyResponse && jsonElement.TryGetProperty("items", out var itemsProperty))
                        {
                            isEmptyResponse = itemsProperty.ValueKind == JsonValueKind.Null ||
                                              (itemsProperty.ValueKind == JsonValueKind.Array && itemsProperty.GetArrayLength() == 0);
                        }

                        parsedData = isEmptyResponse ? null : jsonElement;
                    }
                    else
                    {
                        isEmptyResponse = true;
                    }
                }
                catch
                {
                    // If deserialization fails, fallback to raw response
                    parsedData = responseBody;
                }

                if (isEmptyResponse)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    parsedData = null;
                }

                var statusCode = context.Response.StatusCode;
                var baseResponse = new BaseResponseFilter<object>
                {
                    Success = statusCode is >= 200 and < 300 && !isEmptyResponse,
                    Data = parsedData,
                    ErrorMessage = isEmptyResponse ? "Not found!" : (statusCode >= 400 ? "An error occurred." : null),
                    StatusCode = statusCode
                };

                var jsonResponse = JsonSerializer.Serialize(baseResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                // Restore original response without wrapping
                context.Response.Body = originalBodyStream;
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
        }
    }

}
