namespace CharityHub.Presentation.Controllers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController:BaseController
{
    private readonly IIdentityService _identityService;

    public UserController(IMediator mediator, IIdentityService identityService) : base(mediator)
    {
        _identityService = identityService;
    }
    
    
    /// <summary>
    /// Retrieves the user profile based on the provided authorization token.
    /// </summary>
    /// <returns>A response containing the user profile data.</returns>
    [HttpGet("get-user-profile")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [OutputCache(PolicyName = "Expire20")]  
    [SwaggerOperation(Summary = "Get User Profile", Description = "Retrieves the user profile using the provided authorization token.")]
    [SwaggerResponse(200, "User profile retrieved successfully", typeof(ProfileResponse))] // Replace with your actual response DTO
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get()
    {
        var token = GetTokenFromHeader(); // Assuming this method extracts the token
        var query = new ProfileRequest { Token = token };
        var result = await _identityService.GetUserProfileByToken(query);
        return Ok(result);
    }

    
    
    
}