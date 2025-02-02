namespace CharityHub.Core.Application.Services.Campaigns.Queries.GetAllCampaigns;

using CharityHub.Core.Contract.Primitives.Handlers;
using CharityHub.Core.Contract.Primitives.Models;

using Contract.Campaigns.Queries;
using Contract.Campaigns.Queries.GetAllCampaigns;

public class GetAllCampaignsQueryHandler : IQueryHandler<GetAllCampaignQuery, PagedData<AllCampaignResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;

    public GetAllCampaignsQueryHandler(ICampaignQueryRepository campaignQueryRepository)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }

    public async Task<PagedData<AllCampaignResponseDto>> Handle(GetAllCampaignQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetAllCampaignsAsync(query);
        return result;
    }
}