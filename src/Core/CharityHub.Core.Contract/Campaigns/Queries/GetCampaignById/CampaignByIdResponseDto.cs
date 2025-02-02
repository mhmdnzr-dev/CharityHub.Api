namespace CharityHub.Core.Contract.Campaigns.Queries.GetCampaignById;

public class CampaignByIdResponseDto
{
    public string Title { get; set; }
    public int DonorCount { get; set; }
    public int RemainingDayCount { get; set; }
    public DateTime? StartDateTime { get; set; }
    public decimal? ChargedAmountProgressPercentage { get; set; }
    public decimal? TotalAmount { get; set; }
}