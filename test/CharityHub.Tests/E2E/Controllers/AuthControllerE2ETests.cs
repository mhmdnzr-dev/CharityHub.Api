namespace CharityHub.Tests.E2E.Controllers;

using System.Net;
using System.Text;

using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;
using Core.Contract.Features.Users.Queries.GetVerifyMobileUsers;
using Core.Contract.Primitives.Models;

using Endpoints;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

using Xunit;

public class AuthControllerE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerE2ETests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task SendOtp_ShouldReturnOk_WhenPhoneNumberIsValid()
    {
        var request = new GetRegisterMobileUserQuery
        {
            PhoneNumber = "09123557786"
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<BaseApiResponse<RegisterMobileUserResponseDto>>(responseString);

        responseData.Should().NotBeNull();
        responseData.Success.Should().BeTrue();
        responseData.StatusCode.Should().Be(200);
        responseData.Data.Should().NotBeNull();
        responseData.Data.IsNewUser.Should().BeFalse();
        responseData.Data.IsSMSSent.Should().BeTrue();
    }

    [Fact]
    public async Task VerifyOtp_ShouldReturnOk_WithValidToken()
    {
        var request = new GetVerifyMobileUserQuery
        {
            PhoneNumber = "09123557786",
            OTPCode = "123456"
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/Auth/verify-otp", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<BaseApiResponse<VerifyMobileUserResponseDto>>(responseString);

        responseData.Should().NotBeNull();
        responseData.Success.Should().BeTrue();
        responseData.StatusCode.Should().Be(200);
        responseData.Data.Should().NotBeNull();
        responseData.Data.Token.Should().NotBeNullOrEmpty();
        responseData.Data.PhoneNumber.Should().Be("09123557786");
    }
    
    [Fact]
    public async Task Logout_ShouldReturnUnauthorized_WhenTokenIsInvalid()
    {
        var token = "Bearer invalid_token"; 

        var requestUrl = "/api/v1/Auth/logout";
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Add("Authorization", token);

        var response = await _client.SendAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().BeNullOrEmpty();
    }
}