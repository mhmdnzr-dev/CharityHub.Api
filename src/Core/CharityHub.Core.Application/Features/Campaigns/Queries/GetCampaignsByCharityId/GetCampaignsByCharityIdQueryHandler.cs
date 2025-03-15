namespace CharityHub.Core.Application.Features.Campaigns.Queries.GetCampaignsByCharityId;

using CharityHub.Core.Contract.Primitives.Models;

using Contract.Features.Campaigns.Queries;
using Contract.Features.Campaigns.Queries.GetCampaignsByCharityId;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetCampaignsByCharityIdQueryHandler : QueryHandlerBase<GetCampaignsByCharityIdQuery,
    PagedData<CampaignsByCharityIdResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;


    public GetCampaignsByCharityIdQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, ICampaignQueryRepository campaignQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
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