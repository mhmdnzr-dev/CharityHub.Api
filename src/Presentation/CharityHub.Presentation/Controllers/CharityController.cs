namespace CharityHub.Presentation.Controllers;

using Core.Contract.Charity.Queries;
using Core.Contract.Charity.Queries.GetAllCharities;
using Core.Contract.Charity.Queries.GetCharityById;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[OutputCache(PolicyName = "Expire20")]  
public class CharityController:BaseController
{
    public CharityController(IMediator mediator) : base(mediator)
    {
    }
    
    
    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetAllCharitiesQuery query)
    {
        var result = await _mediator.Send(query); 
        return Ok(result);  
    }
    
    [HttpGet("get-by-id")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetCharityByIdQuery query)
    {
        var result = await _mediator.Send(query); 
        return Ok(result);  
    }
}