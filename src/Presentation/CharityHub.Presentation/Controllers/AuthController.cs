using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CharityHub.Presentation.Controllers;

using Core.Contract.Features.Users.Queries.GetLogoutMobileUsers;
using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;
using Core.Contract.Features.Users.Queries.GetVerifyMobileUsers;
using Core.Contract.Primitives.Models;

using Filters;

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


    [HttpPost("send-otp")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Send OTP", Description = "Sends an OTP to the specified phone number.")]
    [SwaggerResponse(200, "OTP sent successfully", typeof(BaseApiResponse<RegisterMobileUserResponseDto>))]
    public async Task<IActionResult> SendOtp([FromBody] GetRegisterMobileUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpPost("verify-otp")]
    [MapToApiVersion("1.0")]
    [SwaggerOperation(Summary = "Verify OTP",
        Description = "Verifies the OTP for the specified phone number and generates a token if the OTP is correct.")]
    [SwaggerResponse(200, "OTP verified successfully, token generated",
        typeof(BaseApiResponse<VerifyMobileUserResponseDto>))] 
    public async Task<IActionResult> VerifyOtp([FromBody] GetVerifyMobileUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }



    [HttpPost("logout")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout User",
        Description = "Logs out the user by invalidating the provided authorization token.")]
    [SwaggerResponse(200, "Logout successful",
        typeof(BaseApiResponse<LogoutMobileUserResponseDto>))] 
    public async Task<IActionResult> Logout([FromQuery] GetLogoutMobileUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}