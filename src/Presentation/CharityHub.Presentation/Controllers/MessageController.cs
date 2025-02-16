namespace CharityHub.Presentation.Controllers;

using Core.Contract.Messages.Commands.SeenMessage;
using Core.Contract.Messages.Queries.GetMessageByUserIdQuery;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class MessageController : BaseController
{
    public MessageController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("get-messages")]
    [MapToApiVersion("1.0")]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] GetMessageByUserIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
   
    [HttpPut("update-seen-message")]
    [MapToApiVersion("1.0")]
    [Authorize]
    public async Task<IActionResult> Put([FromBody] SeenMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}