namespace CharityHub.Core.Application.Features.Campaigns.Queries.GetCampaignById;

using Contract.Features.Campaigns.Queries;
using Contract.Features.Campaigns.Queries.GetCampaignById;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetCampaignByIdQueryHandler: QueryHandlerBase<GetCampaignByIdQuery, CampaignByIdResponseDto>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;


    public GetCampaignByIdQueryHandler(IMemoryCache cache, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, ICampaignQueryRepository campaignQueryRepository) : base(cache, tokenService, httpContextAccessor)
    {
        _campaignQueryRepository = campaignQueryRepository;
    }
    public override async Task<CampaignByIdResponseDto> Handle(GetCampaignByIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _campaignQueryRepository.GetDetailedById(query);
        return result;
    }
}