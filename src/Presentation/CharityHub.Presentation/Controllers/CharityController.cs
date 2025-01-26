namespace CharityHub.Presentation.Controllers;

using Core.Contract.Charity.Queries;
using Core.Contract.Charity.Queries.GetAllCharities;

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
    
    
    [HttpPost("get-all")]
    [MapToApiVersion("1.0")]
    public IActionResult Get([FromBody] GetAllCharitiesQuery query)
    {
        var result = _mediator.Send(query); 
        return Ok(result);  
    }
}