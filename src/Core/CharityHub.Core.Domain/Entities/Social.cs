namespace CharityHub.Core.Domain.Entities;

using ValueObjects;

public sealed class Social : BaseEntity
{
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }
}
