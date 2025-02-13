namespace CharityHub.Core.Domain.Entities;

using Common;

public sealed class Term : BaseEntity
{
    public string Description { get; private set; }

    // ** Private constructor for EF Core **
    private Term() { }

    // ** Factory Method to Ensure Valid Term Entities **
    public static Term Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters.", nameof(description));

        return new Term
        {
            Description = description.Trim()
        };
    }
}
