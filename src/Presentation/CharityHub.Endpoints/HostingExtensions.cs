namespace CharityHub.Endpoints;

public static class HostingExtensions
{
    public static void AddCORSPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedDomains = configuration.GetSection("AllowedOrigins").Get<string[]>();

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                // The allowed origins will be resolved at runtime
                policy.WithOrigins(allowedDomains)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
    }
}
