

namespace CharityHub.Core.Domain.Entities;

using ValueObjects;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; }




    private readonly List<CampaignCategory> _campaignCategories = new();
    public IReadOnlyCollection<CampaignCategory> CampaignCategories => _campaignCategories.AsReadOnly();


    // ** Private constructor for EF Core **
    private Category() { }

    // ** Factory Method to Ensure Valid Categories **
    public static Category Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        if (name.Length > 10)
            throw new ArgumentException("Category name must not exceed 10 characters.", nameof(name));

        return new Category
        {
            Name = name.Trim()
        };
    }
}
