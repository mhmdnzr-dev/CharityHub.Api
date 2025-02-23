namespace CharityHub.Presentation.Middleware;

using Infra.Identity.Interfaces;
using Infra.Sql.Data.DbContexts;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

internal sealed class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITokenService tokenService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (!string.IsNullOrEmpty(token))
        {
            var isValid = await tokenService.IsTokenValidAsync(token);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or expired token.");
                return;
            }
        }

        await _next(context);
    }
}

