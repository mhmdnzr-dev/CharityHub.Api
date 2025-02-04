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

        if (!context.Items.ContainsKey("SkipBaseResponse")) // Bypass wrapping if flagged
        {
            object parsedData;
            bool isEmptyResponse = false;

            try
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(responseBody);

                // Check if it's null or an empty array
                isEmptyResponse = jsonElement.ValueKind == JsonValueKind.Null ||
                                  (jsonElement.ValueKind == JsonValueKind.Array && jsonElement.GetArrayLength() == 0);

                parsedData = jsonElement;
            }
            catch
            {
                parsedData = responseBody; // Keep raw response if deserialization fails
            }

            if (isEmptyResponse)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                parsedData = null;
            }

            var baseResponse = new BaseResponseFilter<object>
            {
                Success = context.Response.StatusCode is >= 200 and < 300 && !isEmptyResponse,
                Data = parsedData,
                ErrorMessage = isEmptyResponse ? "Not found!" : context.Response.StatusCode >= 400 ? "An error occurred." : null,
                StatusCode = context.Response.StatusCode
            };

            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = baseResponse.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(baseResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
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
