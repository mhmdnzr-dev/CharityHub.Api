namespace CharityHub.Core.Application.Features.Campaigns.Queries.GetAllCampaigns;

using CharityHub.Core.Contract.Primitives.Models;

using Contract.Features.Campaigns.Queries;
using Contract.Features.Campaigns.Queries.GetAllCampaigns;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCampaignsQueryHandler : QueryHandlerBase<GetAllCampaignQuery, PagedData<AllCampaignResponseDto>>
{
    private readonly ICampaignQueryRepository _campaignQueryRepository;


    public GetAllCampaignsQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, ICampaignQueryRepository campaignQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
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