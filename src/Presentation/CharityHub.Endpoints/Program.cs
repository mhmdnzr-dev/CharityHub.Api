
using CharityHub.Endpoints.DependencyInjection;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCORSPolicy(builder.Configuration);

builder.Services.AddIdentityAuthorization();
builder.Services.AddCustomServices();
builder.Services.AddDbContext();

// Learn more about configuring OpenAPI
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapOpenApi();
    app.MapScalarApiReference(op =>
    {
        // Enable authentication for the API reference
        op.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = JwtBearerDefaults.AuthenticationScheme
        };
    });
}

app.UseHttpsRedirection();

// Use CORS middleware
app.UseCors("CorsPolicy");

app.UseRouting();

// Add custom response middleware


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
