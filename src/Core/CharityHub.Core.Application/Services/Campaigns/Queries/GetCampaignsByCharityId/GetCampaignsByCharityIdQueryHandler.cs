namespace CharityHub.Core.Application.Services.Campaigns.Queries.GetCampaignsByCharityId;

using CharityHub.Core.Contract.Primitives.Handlers;
using CharityHub.Core.Contract.Primitives.Models;

using Contract.Campaigns.Queries;
using Contract.Campaigns.Queries.GetCampaignsByCharityId;

public class GetCampaignsByCharityIdQueryHandler: IQueryHandler<GetCampaignsByCharityIdQuery, PagedData<CampaignsByCharityIdResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;

    public GetCampaignsByCharityIdQueryHandler(ICampaignQueryRepository campaignQueryRepository)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }

    public async Task<PagedData<CampaignsByCharityIdResponseDto>> Handle(GetCampaignsByCharityIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetCampaignsByCharityId(query);
        return result;
    }
}