namespace CharityHub.Core.Application.Services.Campaigns.Queries.GetCampaignsByCharityId;

using CharityHub.Core.Contract.Primitives.Handlers;
using CharityHub.Core.Contract.Primitives.Models;

using Contract.Campaigns.Queries;
using Contract.Campaigns.Queries.GetCampaignsByCharityId;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetCampaignsByCharityIdQueryHandler: QueryHandlerBase<GetCampaignsByCharityIdQuery, PagedData<CampaignsByCharityIdResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;


    public GetCampaignsByCharityIdQueryHandler(IMemoryCache cache, ICampaignQueryRepository campaignQueryRepository) : base(cache)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }

    public override async Task<PagedData<CampaignsByCharityIdResponseDto>> Handle(GetCampaignsByCharityIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetCampaignsByCharityId(query);
        return result;
    }
}