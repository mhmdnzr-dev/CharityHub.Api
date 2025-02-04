namespace CharityHub.Presentation.Controllers;

using Core.Contract.Users.Commands.UpdateUserProfiles;
using Core.Contract.Users.Queries.GetUserProfileDetails;



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
    public UserController(IMediator mediator) : base(mediator)
    {
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
        typeof(UserProfileDetailResponseDto))] // Replace with your actual response DTO
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetUserProfileDetailQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    /// <summary>
    /// Updates the user's profile with the provided information.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A response indicating whether the update operation was successful.</returns>
    [HttpPut("update-user-profile")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Update User Profile",
        Description = "Updates the user's profile with the provided information (first name, last name).")]
    [SwaggerResponse(200, "Profile updated successfully", typeof(int))]
    [SwaggerResponse(400, "Bad Request, invalid request data")]
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Put([FromBody] UpdateUserProfileCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}