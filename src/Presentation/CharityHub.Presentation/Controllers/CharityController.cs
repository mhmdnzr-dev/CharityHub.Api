namespace CharityHub.Presentation.Controllers;

using Core.Contract.Features.Charity.Commands.CreateCharity;
using Core.Contract.Features.Charity.Queries.GetAllCharities;
using Core.Contract.Features.Charity.Queries.GetCharityById;
using Core.Contract.Primitives.Models;

using MediatR;

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


    /// <summary>
    /// Retrieves all charities based on the provided query parameters.
    /// </summary>
    /// <param name="query">The query parameters to filter and retrieve charities.</param>
    /// <returns>A response containing the list of charities.</returns>
    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]
    [SwaggerOperation(Summary = "Get All Charities",
        Description = "Retrieves all charities based on the provided query parameters.")]
    [SwaggerResponse(200, "Charities retrieved successfully",
        typeof(PagedData<AllCharitiesResponseDto>))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetAllCharitiesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a charity by its unique identifier.
    /// </summary>
    /// <param name="query">The query parameters containing the charity ID.</param>
    /// <returns>A response containing the charity details.</returns>
    [HttpGet("get-by-id")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]
    [SwaggerOperation(Summary = "Get Charity by ID",
        Description = "Retrieves a charity based on the provided charity ID.")]
    [SwaggerResponse(200, "Charity retrieved successfully",
        typeof(CharityByIdResponseDto))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(404, "Charity not found")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetCharityByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    /// <summary>
    /// Creates a new charity with the provided details.
    /// </summary>
    /// <param name="command">The request containing the charity details.</param>
    /// <returns>The created charity's details or an appropriate error response.</returns>
    [HttpPost("create-charity")]
    [MapToApiVersion("2.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a new Charity",
        Description = "Creates a new charity with the provided name, description, and other details.")]
    [SwaggerResponse(201, "Charity created successfully", typeof(int))]
    [SwaggerResponse(400, "Bad Request, invalid input data")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromBody] CreateCharityCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}