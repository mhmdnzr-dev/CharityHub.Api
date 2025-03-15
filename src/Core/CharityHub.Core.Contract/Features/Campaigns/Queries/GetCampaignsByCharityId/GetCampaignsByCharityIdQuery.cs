namespace CharityHub.Core.Contract.Features.Campaigns.Queries.GetCampaignsByCharityId;



using Primitives.Models;

public class GetCampaignsByCharityIdQuery : PagedQuery<CampaignsByCharityIdResponseDto>
{
    public int Id { get; set; }
}