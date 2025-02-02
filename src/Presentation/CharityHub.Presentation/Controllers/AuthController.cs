using CharityHub.Core.Contract.Authentication;
using CharityHub.Core.Contract.Terms.Queries.GetLastTerm;
using CharityHub.Infra.Identity.Interfaces;
using CharityHub.Infra.Identity.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CharityHub.Presentation.Controllers;

using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[OutputCache(PolicyName = "Expire20")]
public class AuthController : BaseController
{
    private readonly IIdentityService _identityService;

    public AuthController(IMediator mediator, IIdentityService identityService) : base(mediator)
    {
        _identityService = identityService;
    }

    [HttpPost("send-otp")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpQuery query)
    {
        var otpResponse = await _identityService.SendOTPAsync(new SendOtpRequest { PhoneNumber = query.PhoneNumber, });

        SendOtpDto sendOTP = new SendOtpDto { IsNewUser = otpResponse.IsNewUser };
        return Ok(sendOTP);
    }

    [HttpPost("verify-otp")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpQuery query)
    {
        var response = await _identityService.VerifyOTPAndGenerateTokenAsync(new VerifyOtpRequest
        {
            PhoneNumber = query.PhoneNumber, OtpCode = query.Otp,
        });

        VerifyDto verifyDto = new VerifyDto { Token = response.Token, };
        return Ok(verifyDto);
    }


    [HttpGet("get-user-profile")]
    [MapToApiVersion("1.0")]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var token = GetTokenFromHeader();
        var query = new ProfileRequest { Token = token };
        var result =await  _identityService.GetUserProfileByToken(query);
        return Ok(result);
    }

    [HttpGet("last-term")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get([FromQuery] GetLastTermQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpPost("logout")]
    [MapToApiVersion("1.0")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = GetTokenFromHeader();
        var query = new LogoutRequest { Token = token };
        var result = await _identityService.LogoutAsync(query);
        return Ok(result);
    }
}