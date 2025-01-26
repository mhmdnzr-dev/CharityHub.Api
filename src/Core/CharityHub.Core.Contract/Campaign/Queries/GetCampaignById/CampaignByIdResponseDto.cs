namespace CharityHub.Core.Contract.Campaign.Queries.GetCampaignById;

public class CampaignByIdResponseDto
{
    public string Title { get; set; }
    public int DonorCount { get; set; }
    public int RemainingDayCount { get; set; }
    public DateTime? StartDateTime { get; set; }
    public int? ChargedAmountProgressPercentage { get; set; }
    public decimal? TotalAmount { get; set; }
}