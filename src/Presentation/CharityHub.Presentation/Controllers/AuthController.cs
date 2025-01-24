using CharityHub.Core.Contract.Authentication;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CharityHub.Presentation.Controllers;


[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[OutputCache(PolicyName = "Expire20")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("send-otp")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpQuery query)
    {
        var otpResponse = await _identityService.SendOTPAsync(new SendOtpRequest
        {
            PhoneNumber = query.PhoneNumber,
        });

        SendOtpDto sendOTP = new SendOtpDto
        {
            IsNewUser = otpResponse.IsNewUser
        };
        return Ok(sendOTP);
    }

    [HttpPost("verify-otp")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpQuery query)
    {
        var response = await _identityService.VerifyOTPAndGenerateTokenAsync(new VerifyOtpRequest
        {
            PhoneNumber = query.PhoneNumber,
            OtpCode = query.Otp,
        });

        VerifyDto verifyDto = new VerifyDto
        {
            Token = response.Token,
        };
        return Ok(verifyDto);
    }
}

