namespace CharityHub.Presentation.Controllers;

using Core.Contract.Campaigns.Queries.GetAllCampaigns;
using Core.Contract.Categories.Queries.GetAllCategories;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[OutputCache(PolicyName = "Expire20")]
public class CategoryController: BaseController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet("get-all")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetAllCategoriesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
}