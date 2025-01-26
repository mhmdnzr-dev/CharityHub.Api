namespace CharityHub.Core.Contract.Charity.Queries;

using Domain.Entities;

using GetAllCharities;

using GetCharityById;

using Primitives.Repositories;

public interface ICharityQueryRepository : IQueryRepository<Charity>
{
    Task<IEnumerable<AllCharitiesResponseDto>> GetAllAsync(GetAllCharitiesQuery query);
    Task<CharityByIdResponseDto> GetDetailedById(GetCharityByIdQuery query);
}