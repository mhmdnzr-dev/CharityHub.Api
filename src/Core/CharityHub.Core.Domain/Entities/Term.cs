namespace CharityHub.Core.Domain.Entities;

using ValueObjects;

public sealed class Term : BaseEntity
{
    public string Description { get; private set; }
}
