namespace CharityHub.Presentation.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Linq;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the action method has the [Authorize] attribute
        var hasAuthorizeAttribute = context.MethodInfo
            .GetCustomAttributes(true)
            .Any(attr => attr is AuthorizeAttribute);

        // If the action method doesn't have [Authorize], check if the controller has it
        if (!hasAuthorizeAttribute)
        {
            hasAuthorizeAttribute = context.MethodInfo.DeclaringType
                .GetCustomAttributes(true)
                .Any(attr => attr is AuthorizeAttribute);
        }

        // If [Authorize] attribute is found on either the controller or action, add security requirement
        if (hasAuthorizeAttribute)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            // Add the security requirement for the Bearer token (JWT)
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" // The name of the security scheme defined earlier
                        }
                    },
                    new string[] { }
                }
            });
        }
    }
}
