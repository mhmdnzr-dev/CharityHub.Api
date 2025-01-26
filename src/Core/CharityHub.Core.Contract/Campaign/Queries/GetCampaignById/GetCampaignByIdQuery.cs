namespace CharityHub.Core.Contract.Campaign.Queries.GetCampaignById;

using Primitives.Handlers;

public class GetCampaignByIdQuery : IQuery<CampaignByIdResponseDto>
{
    public int Id { get; set; }
}