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
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        return null; // Or handle the case where the token is not available
    }
}
