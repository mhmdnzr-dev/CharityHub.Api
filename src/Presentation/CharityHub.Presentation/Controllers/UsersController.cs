using CharityHub.Infra.Identity.Services;

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;


namespace CharityHub.Presentation.Controllers;



[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly SignupService _signupService;
    private readonly SigninService _signinService;

    public UsersController(SignupService signupService, SigninService signinService)
    {
        _signupService = signupService;
        _signinService = signinService;
    }

    [HttpPost("register")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var success = await _signupService.SignUpAsync(request.Email, request.Password);
        if (success)
        {
            return Ok(new { Message = "User registered successfully." });
        }

        return BadRequest(new { Message = "Failed to register user." });
    }

    [HttpPost("login")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var success = await _signinService.SignInAsync(request.Email, request.Password);
        if (success)
        {
            return Ok(new { Message = "Login successful." });
        }

        return Unauthorized(new { Message = "Invalid email or password." });
    }
}

