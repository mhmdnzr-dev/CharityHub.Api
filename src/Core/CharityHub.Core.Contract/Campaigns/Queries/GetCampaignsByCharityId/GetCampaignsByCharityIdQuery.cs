namespace CharityHub.Core.Contract.Campaigns.Queries.GetCampaignsByCharityId;

using CharityHub.Core.Contract.Primitives.Handlers;

public class GetCampaignsByCharityIdQuery : PagedQuery<CampaignsByCharityIdResponseDto>
{
    public int Id { get; set; }
}