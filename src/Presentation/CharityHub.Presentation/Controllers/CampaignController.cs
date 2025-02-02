namespace CharityHub.Presentation.Controllers;

using Core.Contract.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Campaigns.Queries.GetCampaignById;
using Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;
using Core.Contract.Primitives.Models;

using Swashbuckle.AspNetCore.Annotations;

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


    /// <summary>
    /// Gets all campaigns.
    /// </summary>
    /// <param name="query">The query parameters to filter campaigns.</param>
    /// <returns>A list of campaigns matching the query.</returns>
    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Get all campaigns", Description = "Retrieve a list of campaigns based on query parameters.")]
    [SwaggerResponse(200, "The list of campaigns", typeof(List<AllCampaignResponseDto>))] // Replace CampaignDto with your actual output model
    [SwaggerResponse(400, "Bad Request")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetAllCampaignQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves a campaign by its unique identifier.
    /// </summary>
    /// <param name="query">The query parameters containing the campaign ID.</param>
    /// <returns>A response containing the campaign details.</returns>
    [HttpGet("get-by-id")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Get Campaign by ID", Description = "Retrieves a campaign based on the provided campaign ID.")]
    [SwaggerResponse(200, "Campaign retrieved successfully", typeof(CampaignByIdResponseDto))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(404, "Campaign not found")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetCampaignByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    
    /// <summary>
    /// Retrieves campaigns associated with a specific charity by its unique identifier.
    /// </summary>
    /// <param name="query">The query parameters containing the charity ID.</param>
    /// <returns>A response containing the list of campaigns for the specified charity.</returns>
    [HttpGet("get-by-charity-id")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Get Campaigns by Charity ID", Description = "Retrieves all campaigns associated with a specific charity identified by its charity ID.")]
    [SwaggerResponse(200, "Campaigns retrieved successfully", typeof(PagedData<CampaignsByCharityIdResponseDto>))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(404, "No campaigns found for the specified charity ID")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetCampaignsByCharityIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

