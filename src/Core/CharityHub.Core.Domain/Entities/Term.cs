namespace CharityHub.Core.Domain.Entities;

using ValueObjects;

public sealed class Term : BaseEntity
{
    public required string Description { get; set; }
}
