namespace CharityHub.Core.Application.Services.Charities.Queries.GetAllCharities;

using Contract.Charity.Queries;
using Contract.Charity.Queries.GetAllCharities;
using Contract.Primitives.Handlers;

public class GetAllCharitiesQueryHandler : IQueryHandler<GetAllCharitiesQuery, List<AllCharitiesResponseDto>>
{
    private readonly ICharityQueryRepository _charityQueryRepository;

    public GetAllCharitiesQueryHandler(ICharityQueryRepository charityQueryRepository)
    {
        _charityQueryRepository = charityQueryRepository;
    }

    public async Task<List<AllCharitiesResponseDto>> Handle(GetAllCharitiesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _charityQueryRepository.GetAllAsync(query);
        return result;
    }
}