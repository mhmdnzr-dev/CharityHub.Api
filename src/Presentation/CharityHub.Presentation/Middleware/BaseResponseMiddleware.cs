using Microsoft.AspNetCore.Http;

namespace CharityHub.Presentation.Middleware;

using Core.Contract.Primitives.Models;

using FluentValidation;

using System.Text.Json;

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

            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
                return;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                return;
            }


            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            if (!context.Items.ContainsKey("SkipBaseResponse"))
            {
                object parsedData;
                try
                {
                    parsedData = JsonSerializer.Deserialize<JsonElement>(responseBody);
                }
                catch
                {
                    parsedData = responseBody;
                }

                if (parsedData is JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == JsonValueKind.Array)
                    {
                        if (jsonElement.GetArrayLength() == 0)
                        {
                            await WriteNotFoundResponseAsync(context);
                            return;
                        }
                    }
                    else if (jsonElement.ValueKind == JsonValueKind.Object)
                    {
                        if (jsonElement.TryGetProperty("data", out var dataElement) &&
                            dataElement.TryGetProperty("items", out var itemsElement) &&
                            (itemsElement.ValueKind == JsonValueKind.Null ||
                             (itemsElement.ValueKind == JsonValueKind.Array && itemsElement.GetArrayLength() == 0)))
                        {
                            await WriteNotFoundResponseAsync(context);
                            return;
                        }
                    }
                }

                var baseResponse = new BaseApiResponse<object>
                {
                    Success = context.Response.StatusCode is >= 200 and < 300,
                    Data = parsedData,
                    ErrorMessage = context.Response.StatusCode >= 400 ? "An error occurred." : null,
                    StatusCode = context.Response.StatusCode
                };

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = baseResponse.StatusCode;

                var jsonResponse = JsonSerializer.Serialize(baseResponse,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                context.Response.Body = originalBodyStream;
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
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
            ErrorMessage =
                string.Join("\n", exception.Errors.Select(e => $"-- {e.PropertyName}: {e.ErrorMessage}")),
            StatusCode = StatusCodes.Status400BadRequest
        };

        var jsonResponse = JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return context.Response.WriteAsync(jsonResponse);
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Flag the response to skip BaseResponseMiddleware wrapping
        context.Items["SkipBaseResponse"] = true;

        var response = new BaseApiResponse<string>
        {
            Success = false,
            Data = null,
            ErrorMessage = exception.Message,
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return context.Response.WriteAsync(jsonResponse);
    }

    private static Task WriteNotFoundResponseAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        context.Response.ContentType = "application/json";

        var response = new BaseApiResponse<string>
        {
            Success = false, Data = null, ErrorMessage = "Not found!", StatusCode = StatusCodes.Status404NotFound
        };

        var jsonResponse = JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return context.Response.WriteAsync(jsonResponse);
    }
}