using CharityHub.Core.Domain.Entities;

namespace CharityHub.Core.Domain.ValueObjects;


public class CampaignCategory
{
    public int CampaignId { get; private set; }
    public Campaign Campaign { get; private set; }

    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
}