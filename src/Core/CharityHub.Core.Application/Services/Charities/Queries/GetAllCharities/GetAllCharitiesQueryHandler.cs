namespace CharityHub.Core.Application.Services.Charities.Queries.GetAllCharities;

using Contract.Charity.Queries;
using Contract.Charity.Queries.GetAllCharities;
using Contract.Primitives.Handlers;
using Contract.Primitives.Models;

using Infra.Identity.Interfaces;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCharitiesQueryHandler : QueryHandlerBase<GetAllCharitiesQuery, PagedData<AllCharitiesResponseDto>>
{
    private readonly ICharityQueryRepository _charityQueryRepository;


    public GetAllCharitiesQueryHandler(IMemoryCache cache, ITokenService tokenService, ICharityQueryRepository charityQueryRepository) : base(cache, tokenService)
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