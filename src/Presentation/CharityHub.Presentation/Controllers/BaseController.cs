namespace CharityHub.Presentation.Controllers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected string GetTokenFromHeader()
    {
        if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            var token = authorizationHeader.ToString();

            if (!string.IsNullOrWhiteSpace(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
            }

            return token.Trim(); // Return the original token if "Bearer " is not found
        }

        return null; // Return null if the Authorization header is missing
    }


}
