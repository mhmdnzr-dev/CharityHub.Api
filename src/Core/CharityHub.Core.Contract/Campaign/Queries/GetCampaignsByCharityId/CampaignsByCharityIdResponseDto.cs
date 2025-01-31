namespace CharityHub.Core.Contract.Campaign.Queries.GetCampaignsByCharityId;


public class CampaignsByCharityIdResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? ChargedAmount { get; set; }
    public decimal? Percentage { get; set; }
    public string CharityName { get; set; }
}