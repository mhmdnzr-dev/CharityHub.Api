var builder = DistributedApplication.CreateBuilder(args);

// Configure application without external dependencies
builder.AddProject<Projects.CharityHub_Endpoints>("charityhub-endpoints");

// Configure application without external dependencies
builder.Build().Run();
