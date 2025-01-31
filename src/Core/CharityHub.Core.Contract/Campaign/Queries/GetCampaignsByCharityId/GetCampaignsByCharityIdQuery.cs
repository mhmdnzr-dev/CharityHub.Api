namespace CharityHub.Core.Contract.Campaign.Queries.GetCampaignsByCharityId;

using Primitives.Handlers;


public class GetCampaignsByCharityIdQuery : PagedQuery<CampaignsByCharityIdResponseDto>
{
    public int Id { get; set; }
}