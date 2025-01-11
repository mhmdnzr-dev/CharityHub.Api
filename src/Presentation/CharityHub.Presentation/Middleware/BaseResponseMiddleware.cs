namespace CharityHub.Presentation.Middleware;
using System;
using System.Text.Json;
using System.Threading.Tasks;

using CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Http;

public class BaseResponseMiddleware
{
    private readonly RequestDelegate _next;

    public BaseResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Proceed to the next middleware in the pipeline
            await _next(context);

            // Handle successful responses
            if (context.Response.StatusCode is >= 200 and < 300)
            {
                context.Response.ContentType = "application/json";
                var response = new BaseResponse<object>
                {
                    Success = true,
                    Data = null,
                    ErrorMessage = null,
                    StatusCode = context.Response.StatusCode
                };
                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions and create error response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errorResponse = new BaseResponse<object>
            {
                Success = false,
                Data = null,
                ErrorMessage = ex.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            };

            var errorJson = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(errorJson);
        }
    }
}