namespace CharityHub.Core.Domain.Entities.Charities;
using System.Collections.Generic;

using CharityHub.Core.Domain.ValueObjects;

public class Charity : BaseEntity
{
    public string Name { get; set; }
    public CharityDetails Details { get; private set; }
    public List<CharityCampaign> Campaigns { get; private set; } = new();

    public void AddCampaign(CharityCampaign campaign)
    {
        Campaigns.Add(campaign);
    }
}


