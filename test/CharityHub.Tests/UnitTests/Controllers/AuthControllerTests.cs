namespace CharityHub.Tests.UnitTests.Controllers;

using System.Threading;
using System.Threading.Tasks;

using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;
using Core.Contract.Features.Users.Queries.GetVerifyMobileUsers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Presentation.Controllers;

using Xunit;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
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
            Token = "fake-jwt-token",
            Name = "John Doe",
            PhoneNumber = "09123456789"
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
}