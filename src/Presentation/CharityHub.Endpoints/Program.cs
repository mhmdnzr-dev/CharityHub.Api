using CharityHub.Core.Application.Configuration;
using CharityHub.Endpoints;
using CharityHub.Endpoints.Middleware;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCustomSerilog(builder.Configuration);
builder.Services.AddOptionsConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCORSPolicy(builder.Configuration);


builder.Services.AddIdentityAuthorization();
builder.Services.AddCustomServices();
builder.Services.AddDbContext();

// Learn more about configuring OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Use CORS middleware
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.Run();
