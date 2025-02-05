namespace CharityHub.Core.Contract.Campaigns.Queries.GetAllCampaigns;

public class AllCampaignResponseDto
{
    public string Title { get; set; }
    public string CharityName { get; set; }
    public decimal? ContributionAmount { get; set; }
    public int Id { get; set; }
    public int RemainingDayCount { get; set; }
}