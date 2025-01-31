namespace CharityHub.Presentation.Controllers;

using Core.Contract.Campaign.Queries.GetAllCampaigns;
using Core.Contract.Campaign.Queries.GetCampaignById;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[OutputCache(PolicyName = "Expire20")]
public class CampaignController : BaseController
{
    public CampaignController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetAllCampaignQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    
    [HttpGet("get-by-id")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetCampaignByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}