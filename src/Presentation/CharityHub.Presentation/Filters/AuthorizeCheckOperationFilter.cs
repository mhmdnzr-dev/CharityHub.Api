namespace CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the action or controller has the [Authorize] attribute
        var hasAuthorizeAttribute = context.MethodInfo
            .GetCustomAttributes(true)
            .Any(attr => attr is AuthorizeAttribute);

        if (hasAuthorizeAttribute)
        {
            // Add the security requirement for actions that are authorized
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                }
            };
        }
    }
}
