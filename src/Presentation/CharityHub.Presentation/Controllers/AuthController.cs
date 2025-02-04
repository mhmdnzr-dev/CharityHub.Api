

using MediatR;

using Microsoft.AspNetCore.Mvc;
namespace CharityHub.Presentation.Controllers;

using Core.Contract.Users.Queries.GetLogoutMobileUsers;
using Core.Contract.Users.Queries.GetRegisterMobileUsers;
using Core.Contract.Users.Queries.GetVerifyMobileUsers;

using Microsoft.AspNetCore.Authorization;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Sends an OTP to the specified phone number.
    /// </summary>
    /// <param name="query">The phone number to which the OTP should be sent.</param>
    /// <returns>A response indicating if the user is new and whether the OTP was successfully sent.</returns>
    [HttpPost("send-otp")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Send OTP", Description = "Sends an OTP to the specified phone number.")]
    [SwaggerResponse(200, "OTP sent successfully",
        typeof(RegisterMobileUserResponseDto))] // Replace SendOtpDto with your actual response model
    [SwaggerResponse(400, "Bad Request")] // Optionally add an ErrorDto to handle errors
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> SendOtp([FromBody] GetRegisterMobileUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Verifies the OTP for the specified phone number and generates a token if successful.
    /// </summary>
    /// <param name="query">The phone number and OTP code to verify.</param>
    /// <returns>A response containing a token if OTP verification is successful.</returns>
    [HttpPost("verify-otp")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Verify OTP",
        Description = "Verifies the OTP for the specified phone number and generates a token if the OTP is correct.")]
    [SwaggerResponse(200, "OTP verified successfully, token generated",
        typeof(VerifyMobileUserResponseDto))] // The VerifyDto contains the token
    [SwaggerResponse(400, "Bad Request")] // Optionally, return an ErrorDto for error cases
    [SwaggerResponse(401, "Unauthorized, invalid OTP")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> VerifyOtp([FromBody] GetVerifyMobileUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    /// <summary>
    /// Logs out the user by invalidating the provided authorization token.
    /// </summary>
    /// <returns>A response indicating the success or failure of the logout operation.</returns>
    [HttpPost("logout")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout User",
        Description = "Logs out the user by invalidating the provided authorization token.")]
    [SwaggerResponse(200, "Logout successful", typeof(LogoutMobileUserResponseDto))] // Replace with your actual response DTO
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Logout([FromQuery] GetLogoutMobileUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}