namespace CharityHub.Presentation.Controllers;

using Core.Contract.Features.Campaigns.Commands.CreateCampaignByCharityId;
using Core.Contract.Features.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Features.Campaigns.Queries.GetCampaignById;
using Core.Contract.Features.Campaigns.Queries.GetCampaignsByCharityId;
using Core.Contract.Primitives.Models;

using Swashbuckle.AspNetCore.Annotations;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class CampaignController : BaseController
{
    public CampaignController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]  
    [SwaggerOperation(Summary = "Get all campaigns", Description = "Retrieve a list of campaigns based on query parameters.")]
    [SwaggerResponse(200, "The list of campaigns", typeof(List<AllCampaignResponseDto>))] 
    public async Task<IActionResult> GetAllCampaigns([FromQuery] GetAllCampaignQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-by-id")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]  
    [SwaggerOperation(Summary = "Get Campaign by ID", Description = "Retrieves a campaign based on the provided campaign ID.")]
    [SwaggerResponse(200, "Campaign retrieved successfully", typeof(CampaignByIdResponseDto))] 
    public async Task<IActionResult> GetCampaignById([FromQuery] GetCampaignByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    

    
    [HttpGet("get-by-charity-id")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]  
    [SwaggerOperation(Summary = "Get Campaigns by Charity ID", Description = "Retrieves all campaigns associated with a specific charity identified by its charity ID.")]
    [SwaggerResponse(200, "Campaigns retrieved successfully", typeof(PagedData<CampaignsByCharityIdResponseDto>))] 
    public async Task<IActionResult> GetCampaignsByCharityIdMobile([FromQuery] GetCampaignsByCharityIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("get-by-charity-id")]
    [MapToApiVersion("2.0")]
    [OutputCache(PolicyName = "Expire20")]  
    [SwaggerOperation(Summary = "Get Campaigns by Charity ID", Description = "Retrieves all campaigns associated with a specific charity identified by its charity ID.")]
    [SwaggerResponse(200, "Campaigns retrieved successfully", typeof(PagedData<CampaignsByCharityIdResponseDto>))] 
    public async Task<IActionResult> GetCampaignsByCharityIdPanel([FromQuery] GetCampaignsByCharityIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    
    [Authorize]
    [MapToApiVersion("2.0")]
    [HttpPost("create-campaign-by-charity-id")]
    [SwaggerResponse(200, "Campaigns added successfully", typeof(int))] 
    public async Task<IActionResult> Post([FromBody] CreateCampaignByCharityIdCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

