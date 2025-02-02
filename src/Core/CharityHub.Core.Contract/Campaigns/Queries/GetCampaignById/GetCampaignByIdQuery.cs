namespace CharityHub.Core.Contract.Campaigns.Queries.GetCampaignById;

using CharityHub.Core.Contract.Primitives.Handlers;

public class GetCampaignByIdQuery : IQuery<CampaignByIdResponseDto>
{
    public int Id { get; set; }
}