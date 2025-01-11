using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CharityHub.Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[OutputCache(PolicyName = "Expire20")]
public class ProductsController : ControllerBase
{
    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    public IActionResult GetV1(int id)
    {
        return Ok(new { Message = "Version 1", ProductId = id });
    }

    [HttpGet("{id}")]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2(int id)
    {
        return Ok(new { Message = "Version 2", ProductId = id });
    }

}

