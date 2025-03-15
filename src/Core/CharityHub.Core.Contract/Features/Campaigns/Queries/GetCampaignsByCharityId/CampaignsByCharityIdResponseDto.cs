namespace CharityHub.Core.Contract.Features.Campaigns.Queries.GetCampaignsByCharityId;

using Domain.Enums;

public class CampaignsByCharityIdResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? ChargedAmount { get; set; }
    public decimal? ChargedAmountProgressPercentage { get; set; }
    public string CharityName { get; set; }
    public CampaignStatus CampaignStatus { get; set; }
    public string BannerUriAddress { get; set; }
    public string CharityLogoUriAddress { get; set; }
}