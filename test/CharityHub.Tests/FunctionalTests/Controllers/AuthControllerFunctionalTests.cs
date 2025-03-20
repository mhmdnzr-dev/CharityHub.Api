namespace CharityHub.Tests.FunctionalTests.Controllers;

using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;

using MediatR;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Presentation.Controllers;

using Xunit;

public sealed class AuthControllerFunctionalTests
{
    private readonly Mock<IMediator> _mediatorMock;

    private readonly AuthController _controller;

    public AuthControllerFunctionalTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }


    [Fact]
    public async Task SendOtp_ReturnsOk_WhenValidRequest()
    {
        // Arrange
        var query = new GetRegisterMobileUserQuery { PhoneNumber = "09123456789" };
        var expectedResponse = new RegisterMobileUserResponseDto { IsNewUser = true, IsSMSSent = true };


        _mediatorMock.Setup(m => m.Send(It.IsAny<GetRegisterMobileUserQuery>(), default))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.SendOtp(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<RegisterMobileUserResponseDto>(okResult.Value);
        Assert.True(response.IsNewUser);
        Assert.True(response.IsSMSSent);
    }
}