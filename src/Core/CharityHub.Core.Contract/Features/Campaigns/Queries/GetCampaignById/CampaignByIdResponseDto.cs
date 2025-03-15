namespace CharityHub.Core.Contract.Features.Campaigns.Queries.GetCampaignById;

using Domain.Enums;

public class CampaignByIdResponseDto
{
    public string Title { get; set; }
    public int RemainingDayCount { get; set; }
    public DateTime? StartDateTime { get; set; }
    public decimal? ChargedAmountProgressPercentage { get; set; }
    public decimal? TotalAmount { get; set; }
    public int ContributionAmount { get; set; }
    public DateTime? EndDateTime { get; set; }
    public decimal? ChargedAmount { get; set; }
    public string Description { get; set; }
    public int Id { get; set; }
    public string CharityName { get; set; }
    public int CharityId { get; set; }
    public CampaignStatus CampaignStatus { get; set; }
    public string BannerUriAddress { get; set; }
    public string CharityLogoUriAddress { get; set; }
}