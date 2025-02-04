namespace CharityHub.Core.Application.Services.Campaigns.Queries.GetCampaignById;

using CharityHub.Core.Contract.Primitives.Handlers;

using Contract.Campaigns.Queries;
using Contract.Campaigns.Queries.GetCampaignById;

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