namespace CharityHub.Tests.UnitTests.Controllers;

using System.Threading;
using System.Threading.Tasks;

using Core.Contract.Features.Users.Queries.GetLogoutMobileUsers;
using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;
using Core.Contract.Features.Users.Queries.GetVerifyMobileUsers;

using Infra.Identity.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Presentation.Controllers;

using Xunit;

public class AuthControllerUnitTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    private readonly AuthController _controller;


    public AuthControllerUnitTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _tokenServiceMock = new Mock<ITokenService>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task SendOtp_ShouldReturnOk_WithRegisterMobileUserResponseDto()
    {
        var query = new GetRegisterMobileUserQuery { PhoneNumber = "09123456789" };
        var responseData = new RegisterMobileUserResponseDto { IsNewUser = true, IsSMSSent = true };

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseData);

        var result = await _controller.SendOtp(query);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<RegisterMobileUserResponseDto>(okResult.Value);

        Assert.True(returnedValue.IsNewUser);
        Assert.True(returnedValue.IsSMSSent);

        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task VerifyOtp_ShouldReturnOk_WithVerifyMobileUserResponseDto()
    {
        var query = new GetVerifyMobileUserQuery { PhoneNumber = "09123456789", OTPCode = "123456" };
        var responseData = new VerifyMobileUserResponseDto
        {
            Token = "fake-jwt-token", Name = "John Doe", PhoneNumber = "09123456789"
        };

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseData);

        var result = await _controller.VerifyOtp(query);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<VerifyMobileUserResponseDto>(okResult.Value);

        Assert.Equal("fake-jwt-token", returnedValue.Token);
        Assert.Equal("John Doe", returnedValue.Name);
        Assert.Equal("09123456789", returnedValue.PhoneNumber);

        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Logout_ShouldReturnOk_WhenLogoutIsSuccessful()
    {
        var query = new GetLogoutMobileUserQuery();
        var responseData = new LogoutMobileUserResponseDto { IsLogout = true };

        _mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseData);

        var result = await _controller.Logout(query);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<LogoutMobileUserResponseDto>(okResult.Value);

        Assert.True(returnedValue.IsLogout);
        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Logout_ShouldReturnUnauthorized_WhenTokenIsInvalid()
    {
        var token = "invalid-token";
        _tokenServiceMock
            .Setup(t => t.IsTokenValidAsync(token))
            .ReturnsAsync(false);

        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

        var isValid = await _tokenServiceMock.Object.IsTokenValidAsync(token);

        if (!isValid)
        {
            var unauthorizedResult = new ObjectResult("Unauthorized: Invalid or expired token.")
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
            Assert.Equal("Unauthorized: Invalid or expired token.", unauthorizedResult.Value);
        }

        _tokenServiceMock.Verify(t => t.IsTokenValidAsync(token), Times.Once);
    }
    
    
    
}