namespace CharityHub.Presentation.Controllers;

using Core.Contract.Terms.Queries.GetLastTerm;

using Infra.Identity.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TermController : BaseController
{
    
    public TermController(IMediator mediator) : base(mediator)
    {
    }
    
    
    /// <summary>
    /// Retrieves the last term based on the provided query parameters.
    /// </summary>
    /// <param name="query">The query parameters to filter the last term data.</param>
    /// <returns>A response containing the last term data.</returns>
    [HttpGet("last-term")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]
    [SwaggerOperation(Summary = "Get Last Term",
        Description = "Retrieves the last term based on the provided query parameters.")]
    [SwaggerResponse(200, "Last term data retrieved successfully",
        typeof(LastTermResponseDto))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetLastTermQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}