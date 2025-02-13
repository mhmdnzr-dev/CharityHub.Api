namespace CharityHub.Core.Domain.Entities;

using Common;

public sealed class Social : BaseEntity
{
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }
    public string Url { get; private set; }

    // ** Private constructor for EF Core **
    private Social() { }

    // ** Factory Method to Ensure Valid Social Entities **
    public static Social Create(string name, string abbreviation, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Social name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(abbreviation))
            throw new ArgumentException("Social abbreviation is required.", nameof(abbreviation));

        if (abbreviation.Length > 10)
            throw new ArgumentException("Social abbreviation must not exceed 10 characters.", nameof(abbreviation));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Social URL is required.", nameof(url));

        return new Social
        {
            Name = name.Trim(),
            Abbreviation = abbreviation.Trim().ToUpper(),
            Url = url.Trim()  // Ensure URL is trimmed
        };
    }
}
