using System.Net;
using System.Text.Json;

namespace CharityHub.Endpoints.Middleware;
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred during the request.");

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Message = "An unexpected error occurred. Please try again later.",
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Details = context.IsDevelopment() ? exception.Message : null // Fixed here
        };

        response.StatusCode = errorResponse.StatusCode;

        var json = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(json);
    }
}

internal static class HttpContextExtensions
{
    public static bool IsDevelopment(this HttpContext context)
    {
        var environment = context.RequestServices.GetService<IWebHostEnvironment>();
        return environment?.IsDevelopment() ?? false;
    }
}

public class ErrorResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public string? Details { get; set; }
}