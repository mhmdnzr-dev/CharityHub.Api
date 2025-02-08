namespace CharityHub.Core.Domain.ValueObjects;

using CharityHub.Core.Domain.Entities;

public sealed class CampaignCategory
{
    public int CampaignId { get; private set; }
    public Campaign Campaign { get; private set; }

    public int CategoryId { get; private set; }
    public Category Category { get; private set; }

    // Private constructor for EF Core
    private CampaignCategory() { }

    // Factory method to ensure valid CampaignCategory
    public static CampaignCategory Create(int campaignId, int categoryId)
    {
        if (campaignId <= 0)
            throw new ArgumentException("CampaignId must be greater than zero.", nameof(campaignId));

        if (categoryId <= 0)
            throw new ArgumentException("CategoryId must be greater than zero.", nameof(categoryId));

        return new CampaignCategory
        {
            CampaignId = campaignId,
            CategoryId = categoryId
        };
    }
}

