namespace CharityHub.Presentation.Controllers;

using Core.Contract.Features.Categories.Queries.GetAllCategories;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CategoryController: BaseController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }


    /// <summary>
    /// Retrieves all categories based on the provided query parameters.
    /// </summary>
    /// <param name="query">The query parameters to filter and retrieve categories.</param>
    /// <returns>A response containing the list of categories.</returns>
    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    [OutputCache(PolicyName = "Expire20")]  
    [SwaggerOperation(Summary = "Get All Categories", Description = "Retrieves all categories based on the provided query parameters.")]
    [SwaggerResponse(200, "Categories retrieved successfully", typeof(List<AllCategoriesResponseDto>))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetAllCategoriesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
}