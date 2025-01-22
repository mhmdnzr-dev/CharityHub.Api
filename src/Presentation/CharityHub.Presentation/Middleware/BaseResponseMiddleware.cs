using System.Text.Json;

using CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Http;

namespace CharityHub.Presentation.Middleware;

public sealed class BaseResponseMiddleware
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

            string parsedData;
            try
            {
                parsedData = JsonSerializer.Deserialize<JsonElement>(responseBody).ToString();
            }
            catch
            {
                parsedData = responseBody;
            }

            var baseResponse = new BaseResponse<object>
            {
                Success = context.Response.StatusCode is >= 200 and < 300,
                Data = parsedData,
                ErrorMessage = context.Response.StatusCode >= 400 ? "An error occurred." : null,
                StatusCode = context.Response.StatusCode
            };

            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = baseResponse.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(baseResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
