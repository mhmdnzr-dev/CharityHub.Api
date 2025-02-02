namespace CharityHub.Presentation.Controllers;

using Infra.Identity.Interfaces;
using Infra.Identity.Models;
using Infra.Identity.Models.Identity.Requests;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController : BaseController
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
    [SwaggerOperation(Summary = "Get User Profile",
        Description = "Retrieves the user profile using the provided authorization token.")]
    [SwaggerResponse(200, "User profile retrieved successfully",
        typeof(ProfileResponse))] // Replace with your actual response DTO
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get()
    {
        var token = GetTokenFromHeader(); // Assuming this method extracts the token
        var query = new ProfileRequest { Token = token };
        var result = await _identityService.GetUserProfileByToken(query);
        return Ok(result);
    }


    /// <summary>
    /// Updates the user's profile with the provided information.
    /// </summary>
    /// <param name="request">The profile information to update.</param>
    /// <returns>A response indicating whether the update operation was successful.</returns>
    [HttpPut("update-user-profile")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Update User Profile", Description = "Updates the user's profile with the provided information (first name, last name).")]
    [SwaggerResponse(200, "Profile updated successfully", typeof(bool))]
    [SwaggerResponse(400, "Bad Request, invalid request data")]
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Put([FromBody] UpateProfileRequest request)
    {
        var token = GetTokenFromHeader();
        var result = await _identityService.UpdateProfileAsync(request, token);
        return Ok(result);
    }
}