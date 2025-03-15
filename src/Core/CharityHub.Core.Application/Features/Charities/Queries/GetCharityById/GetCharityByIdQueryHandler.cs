namespace CharityHub.Core.Application.Features.Charities.Queries.GetCharityById;

using Contract.Features.Charity.Queries;
using Contract.Features.Charity.Queries.GetCharityById;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetCharityByIdQueryHandler : QueryHandlerBase<GetCharityByIdQuery, CharityByIdResponseDto>
{
    private readonly ICharityQueryRepository _charityQueryRepository;


    public GetCharityByIdQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, ICharityQueryRepository charityQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
    {
        _charityQueryRepository = charityQueryRepository;
    }


    public override async Task<CharityByIdResponseDto> Handle(GetCharityByIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _charityQueryRepository.GetDetailedById(query);
        return result;
    }
}