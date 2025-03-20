namespace CharityHub.Presentation.Middleware;

using System.Text.Json;

using Core.Contract.Primitives.Models;

using FluentValidation;

using Microsoft.AspNetCore.Http;

internal sealed class FluentValidationResponseMiddleware
{
    private readonly RequestDelegate _next;

    public FluentValidationResponseMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var newBodyStream = new MemoryStream())
        {
            context.Response.Body = newBodyStream;

            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }

            await newBodyStream.CopyToAsync(originalBodyStream);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new BaseApiResponse<string>
        {
            Success = false,
            Data = null,
            ErrorMessage = $"Validation failed: \n {string.Join("\n", exception.Errors.Select(e => $"-- {e.PropertyName}: {e.ErrorMessage}"))}",
            StatusCode = StatusCodes.Status400BadRequest
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(jsonResponse);
    }
}
