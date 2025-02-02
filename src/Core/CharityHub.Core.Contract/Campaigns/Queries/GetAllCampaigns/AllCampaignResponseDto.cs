namespace CharityHub.Core.Contract.Campaigns.Queries.GetAllCampaigns;

public class AllCampaignResponseDto
{
    public string Name { get; set; }
    public string CharityName { get; set; }
    public decimal? ContributionAmount { get; set; }
    public DateTime? StartDateTime { get; set; }
}