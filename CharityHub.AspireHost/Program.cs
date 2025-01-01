using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Configure application without external dependencies
builder.AddProject<CharityHub_Endpoints>("charityhub-endpoints");

// Configure application without external dependencies
builder.Build().Run();
