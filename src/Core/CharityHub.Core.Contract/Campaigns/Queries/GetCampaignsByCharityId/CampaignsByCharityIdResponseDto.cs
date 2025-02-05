namespace CharityHub.Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;


public class CampaignsByCharityIdResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? ChargedAmount { get; set; }
    public decimal? ChargedAmountProgressPercentage { get; set; }
    public string CharityName { get; set; }
}