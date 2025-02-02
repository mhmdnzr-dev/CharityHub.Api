using CharityHub.Core.Contract.Authentication;
using CharityHub.Core.Contract.Terms.Queries.GetLastTerm;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CharityHub.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController : BaseController
{
    private readonly IIdentityService _identityService;

    public AuthController(IMediator mediator, IIdentityService identityService) : base(mediator)
    {
        _identityService = identityService;
    }
    
    
    /// <summary>
    /// Sends an OTP to the specified phone number.
    /// </summary>
    /// <param name="query">The phone number to which the OTP should be sent.</param>
    /// <returns>A response indicating if the user is new and whether the OTP was successfully sent.</returns>
    [HttpPost("send-otp")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Send OTP", Description = "Sends an OTP to the specified phone number.")]
    [SwaggerResponse(200, "OTP sent successfully", typeof(SendOtpDto))] // Replace SendOtpDto with your actual response model
    [SwaggerResponse(400, "Bad Request")] // Optionally add an ErrorDto to handle errors
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpQuery query)
    {
        var otpResponse = await _identityService.SendOTPAsync(new SendOtpRequest { PhoneNumber = query.PhoneNumber });

        SendOtpDto sendOTP = new SendOtpDto { IsNewUser = otpResponse.IsNewUser };
        return Ok(sendOTP);
    }

    /// <summary>
    /// Verifies the OTP for the specified phone number and generates a token if successful.
    /// </summary>
    /// <param name="query">The phone number and OTP code to verify.</param>
    /// <returns>A response containing a token if OTP verification is successful.</returns>
    [HttpPost("verify-otp")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Verify OTP", Description = "Verifies the OTP for the specified phone number and generates a token if the OTP is correct.")]
    [SwaggerResponse(200, "OTP verified successfully, token generated", typeof(VerifyDto))] // The VerifyDto contains the token
    [SwaggerResponse(400, "Bad Request")] // Optionally, return an ErrorDto for error cases
    [SwaggerResponse(401, "Unauthorized, invalid OTP")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpQuery query)
    {
        var response = await _identityService.VerifyOTPAndGenerateTokenAsync(new VerifyOtpRequest
        {
            PhoneNumber = query.PhoneNumber, OtpCode = query.Otp,
        });

        VerifyDto verifyDto = new VerifyDto { Token = response.Token, };
        return Ok(verifyDto);
    }



 


    /// <summary>
    /// Logs out the user by invalidating the provided authorization token.
    /// </summary>
    /// <returns>A response indicating the success or failure of the logout operation.</returns>
    [HttpPost("logout")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout User", Description = "Logs out the user by invalidating the provided authorization token.")]
    [SwaggerResponse(200, "Logout successful", typeof(bool))] // Replace with your actual response DTO
    [SwaggerResponse(401, "Unauthorized, invalid or missing token")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Logout()
    {
        var token = GetTokenFromHeader();
        var query = new LogoutRequest { Token = token };
        var result = await _identityService.LogoutAsync(query);
        return Ok(result);
    }
}