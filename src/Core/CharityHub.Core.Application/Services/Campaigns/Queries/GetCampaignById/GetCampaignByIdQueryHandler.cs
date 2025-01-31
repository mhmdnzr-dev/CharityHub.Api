namespace CharityHub.Core.Application.Services.Campaigns.Queries.GetCampaignById;

using CharityHub.Core.Contract.Campaign.Queries;
using CharityHub.Core.Contract.Campaign.Queries.GetCampaignById;
using CharityHub.Core.Contract.Primitives.Handlers;

public class GetCampaignByIdQueryHandler: IQueryHandler<GetCampaignByIdQuery, CampaignByIdResponseDto>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;

    public GetCampaignByIdQueryHandler(ICampaignQueryRepository campaignQueryRepository)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }

    public async Task<CampaignByIdResponseDto> Handle(GetCampaignByIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetDetailedById(query);
        return result;
    }
}