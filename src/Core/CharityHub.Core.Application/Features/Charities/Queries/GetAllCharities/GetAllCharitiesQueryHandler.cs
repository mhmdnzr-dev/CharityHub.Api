namespace CharityHub.Core.Application.Features.Charities.Queries.GetAllCharities;

using Contract.Features.Charity.Queries;
using Contract.Features.Charity.Queries.GetAllCharities;
using Contract.Primitives.Models;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCharitiesQueryHandler : QueryHandlerBase<GetAllCharitiesQuery, PagedData<AllCharitiesResponseDto>>
{
    private readonly ICharityQueryRepository _charityQueryRepository;


    public GetAllCharitiesQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, ICharityQueryRepository charityQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
    {
        _charityQueryRepository = charityQueryRepository;
    }

    public override async Task<PagedData<AllCharitiesResponseDto>> Handle(GetAllCharitiesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _charityQueryRepository.GetAllAsync(query);
        return result;
    }
}