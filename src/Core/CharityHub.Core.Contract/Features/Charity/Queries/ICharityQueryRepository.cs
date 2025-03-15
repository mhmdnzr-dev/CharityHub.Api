namespace CharityHub.Core.Contract.Features.Charity.Queries;

using Domain.Entities;

using GetAllCharities;

using GetCharityById;

using Primitives.Models;
using Primitives.Repositories;

public interface ICharityQueryRepository : IQueryRepository<Charity>
{
    Task<PagedData<AllCharitiesResponseDto>> GetAllAsync(GetAllCharitiesQuery query);
    Task<CharityByIdResponseDto> GetDetailedById(GetCharityByIdQuery dto);
}