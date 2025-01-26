namespace CharityHub.Core.Application.Services.Campaign.Queries.GetAllCampaigns;

using Contract.Campaign.Queries;
using Contract.Campaign.Queries.GetAllCamaigns;
using Contract.Charity.Queries;
using Contract.Charity.Queries.GetAllCharities;
using Contract.Primitives.Handlers;

public class GetAllCampaignsQueryHandler : IQueryHandler<GetAllCampaignQuery, IEnumerable<AllCampaignResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;

    public GetAllCampaignsQueryHandler(ICampaignQueryRepository campaignQueryRepository)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }

    public async Task<IEnumerable<AllCampaignResponseDto>> Handle(GetAllCampaignQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetAllCampaignsAsync(query);
        return result;
    }
}