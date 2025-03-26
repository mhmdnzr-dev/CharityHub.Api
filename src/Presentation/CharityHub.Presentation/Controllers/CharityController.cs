namespace CharityHub.Presentation.Controllers;

using Core.Contract.Features.Charity.Commands.CreateCharity;
using Core.Contract.Features.Charity.Queries.GetAllCharities;
using Core.Contract.Features.Charity.Queries.GetCharityById;
using Core.Contract.Primitives.Models;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.OutputCaching;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class CharityController : BaseController
{
    public CharityController(IMediator mediator) : base(mediator)
    {
    }


    
    
    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Get All Charities",
        Description = "Retrieves all charities based on the provided query parameters.")]
    [SwaggerResponse(200, "Charities retrieved successfully",
        typeof(PagedData<AllCharitiesResponseDto>))] 
    public async Task<IActionResult> GetCharitiesMobile([FromQuery] GetAllCharitiesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("get-all")]
    [MapToApiVersion("2.0")]
    [SwaggerOperation(Summary = "Get All Charities",
        Description = "Retrieves all charities based on the provided query parameters.")]
    [SwaggerResponse(200, "Charities retrieved successfully",
        typeof(PagedData<AllCharitiesResponseDto>))] 
    public async Task<IActionResult> GetCharitiesPanel([FromQuery] GetAllCharitiesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("get-by-id")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]
    [SwaggerOperation(Summary = "Get Charity by ID",
        Description = "Retrieves a charity based on the provided charity ID.")]
    [SwaggerResponse(200, "Charity retrieved successfully",
        typeof(CharityByIdResponseDto))] 
    public async Task<IActionResult> GetCharityByIdMobile([FromQuery] GetCharityByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    
    [HttpGet("get-by-id")]
    [MapToApiVersion("2.0")]
    [Authorize]
    [OutputCache(PolicyName = "Expire20")]
    [SwaggerOperation(Summary = "Get Charity by ID",
        Description = "Retrieves a charity based on the provided charity ID.")]
    [SwaggerResponse(200, "Charity retrieved successfully",
        typeof(CharityByIdResponseDto))] 
    public async Task<IActionResult> GetCharityByIdPanel([FromQuery] GetCharityByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


  
    [HttpPost("create-charity")]
    [MapToApiVersion("2.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a new Charity",
        Description = "Creates a new charity with the provided name, description, and other details.")]
    [SwaggerResponse(201, "Charity created successfully", typeof(int))]
    public async Task<IActionResult> Get([FromBody] CreateCharityCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}