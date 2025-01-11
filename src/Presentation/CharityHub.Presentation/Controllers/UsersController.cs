using CharityHub.Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;


namespace CharityHub.Presentation.Controllers;



[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[OutputCache(PolicyName = "Expire20")]
public class UsersController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public UsersController(IIdentityService identityService)
    {
        _identityService = identityService;
    }


    [HttpPost("register")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var success = await _identityService.SignUpAsync(request.Email, request.Password);
        if (success)
        {
            return Ok(new { Data = "User registered successfully." });
        }

        return BadRequest(new { Data = "Failed to register user." });
    }

    [HttpPost("login")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, token) = await _identityService.SignInAsync(request.Email, request.Password);
        if (success)
        {
            return Ok(new
            {
                Data = "Login successful.",
                Token = token
            });
        }

        return Unauthorized(new { Data = "Invalid email or password." });
    }
}

