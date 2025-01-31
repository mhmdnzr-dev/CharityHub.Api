namespace CharityHub.Core.Application.Services.Campaign.Queries.GetAllCampaigns;

using Contract.Campaign.Queries;
using Contract.Campaign.Queries.GetAllCampaigns;
using Contract.Primitives.Handlers;
using Contract.Primitives.Models;

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