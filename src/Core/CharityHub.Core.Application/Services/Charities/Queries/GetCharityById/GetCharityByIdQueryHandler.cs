namespace CharityHub.Core.Application.Services.Charities.Queries.GetCharityById;

using Contract.Charity.Queries;
using Contract.Charity.Queries.GetAllCharities;
using Contract.Charity.Queries.GetCharityById;
using Contract.Primitives.Handlers;

public class GetCharityByIdQueryHandler: IQueryHandler<GetCharityByIdQuery, CharityByIdResponseDto>
{
    private readonly ICharityQueryRepository _charityQueryRepository;

    public GetCharityByIdQueryHandler(ICharityQueryRepository charityQueryRepository)
    {
        _charityQueryRepository = charityQueryRepository;
    }

    public async Task<CharityByIdResponseDto> Handle(GetCharityByIdQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _charityQueryRepository.GetDetailedById(query);
        return result;
    }
}