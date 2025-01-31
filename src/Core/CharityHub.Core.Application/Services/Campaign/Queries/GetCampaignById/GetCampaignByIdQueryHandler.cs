namespace CharityHub.Core.Application.Services.Campaign.Queries.GetCampaignById;

using Contract.Campaign.Queries;
using Contract.Campaign.Queries.GetCampaignById;
using Contract.Primitives.Handlers;

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