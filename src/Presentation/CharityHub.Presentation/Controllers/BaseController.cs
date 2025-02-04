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
}
