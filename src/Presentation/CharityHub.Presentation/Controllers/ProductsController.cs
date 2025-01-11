using Microsoft.AspNetCore.Mvc;

namespace CharityHub.Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProductsController : ControllerBase
{
    [HttpGet("v1/{id}")]
    [MapToApiVersion("1.0")]
    public IActionResult GetV1(int id)
    {
        return Ok(new { Message = "Version 1", ProductId = id });
    }

    [HttpGet("v2/{id}")]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2(int id)
    {
        return Ok(new { Message = "Version 2", ProductId = id });
    }

}

