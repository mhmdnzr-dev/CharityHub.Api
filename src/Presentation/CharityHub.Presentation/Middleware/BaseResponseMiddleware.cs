using System.Text.Json;

using CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Http;

namespace CharityHub.Presentation.Middleware;


public sealed class BaseResponseMiddleware
{
    private readonly RequestDelegate _next;

    public BaseResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        try
        {
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // اجرای درخواست بعدی
            await _next(context);

            // بررسی اینکه پاسخ هنوز شروع نشده باشد
            if (!context.Response.HasStarted)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var originalResponseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                var response = new BaseResponse<object>
                {
                    Success = context.Response.StatusCode == StatusCodes.Status200OK,
                    Data = JsonSerializer.Deserialize<object>(originalResponseBody),
                    ErrorMessage = context.Response.StatusCode != StatusCodes.Status200OK ? "An error occurred." : null,
                    StatusCode = context.Response.StatusCode
                };

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new BaseResponse<object>
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}
