namespace CharityHub.Core.Contract.Primitives.Models;

public class StaticFileDto
{
    public required string StaticFilePath { get; set; }
    public required string RequestPath { get; set; }
}