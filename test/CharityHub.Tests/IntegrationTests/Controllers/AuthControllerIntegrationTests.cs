namespace CharityHub.Tests.IntegrationTests;

using System.Net;
using System.Text;

using Core.Contract.Features.Users.Queries.GetRegisterMobileUsers;

using Endpoints;

using Infra.Sql.Data.DbContexts;
using Infra.Sql.Data.SeedData;
using Infra.Sql.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Newtonsoft.Json;

using Serilog;

using Xunit;

public class AuthControllerIntegrationTests
{
    private readonly TestServer _server;
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests()
    {
        var builder = WebApplication.CreateBuilder();

        // Register services and middleware from Program.cs
        builder.Host.UseSerilog();
        builder.Services.AddHttpClient();
        builder.Services.AddMemoryCache();
        builder.Services.AddCustomServices(builder.Configuration);
        builder.Services.AddSeeder<DatabaseSeeder>();

        var app = builder.Build();

        // Apply migrations and seed data
        app.Services.SeedAsync<CharityHubCommandDbContext>().Wait();

        // Now initialize the TestServer correctly with IServiceProvider
        _server = new TestServer(app.Services);
        _client = _server.CreateClient();
    }

    [Fact]
    public async Task SendOtp_ReturnsInternalServerError_WhenPhoneNumberIsInvalid()
    {
        var query = new GetRegisterMobileUserQuery { PhoneNumber = "" };
        var content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/auth/send-otp", content);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("Phone number is not valid!", responseBody);
    }
}