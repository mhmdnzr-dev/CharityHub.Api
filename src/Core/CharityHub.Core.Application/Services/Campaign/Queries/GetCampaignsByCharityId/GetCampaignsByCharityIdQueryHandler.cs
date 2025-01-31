namespace CharityHub.Core.Application.Services.Campaign.Queries.GetCampaignsByCharityId;

using Contract.Campaign.Queries;
using Contract.Campaign.Queries.GetCampaignsByCharityId;
using Contract.Primitives.Handlers;
using Contract.Primitives.Models;

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