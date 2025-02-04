namespace CharityHub.Core.Application.Services.Campaigns.Queries.GetAllCampaigns;

using CharityHub.Core.Contract.Primitives.Handlers;
using CharityHub.Core.Contract.Primitives.Models;

using Contract.Campaigns.Queries;
using Contract.Campaigns.Queries.GetAllCampaigns;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCampaignsQueryHandler : QueryHandlerBase<GetAllCampaignQuery, PagedData<AllCampaignResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;


    public GetAllCampaignsQueryHandler(IMemoryCache cache, ICampaignQueryRepository campaignQueryRepository) : base(cache)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }

    public override async Task<PagedData<AllCampaignResponseDto>> Handle(GetAllCampaignQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetAllCampaignsAsync(query);
        return result;
    }
}