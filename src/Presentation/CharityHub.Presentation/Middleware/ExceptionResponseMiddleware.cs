using System.Text.Json;

using CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Http;


namespace CharityHub.Presentation.Middleware;

internal sealed class ExceptionResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionResponseMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Flag the response to skip BaseResponseMiddleware wrapping
        context.Items["SkipBaseResponse"] = true;

        var response = new BaseResponse<string>
        {
            Success = false,
            Data = null,
            ErrorMessage = exception.Message,
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return context.Response.WriteAsync(jsonResponse);
    }
}
