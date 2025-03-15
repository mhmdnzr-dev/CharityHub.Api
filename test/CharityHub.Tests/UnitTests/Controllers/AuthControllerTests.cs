namespace CharityHub.Tests.UnitTests.Controllers;

using System.Threading;
using System.Threading.Tasks;

using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;

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

}