using CharityHub.Core.Application.Configuration;
using CharityHub.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCustomSerilog(builder.Configuration);
builder.Services.AddApplicationConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCORSPolicy(builder.Configuration);

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

app.Run();
