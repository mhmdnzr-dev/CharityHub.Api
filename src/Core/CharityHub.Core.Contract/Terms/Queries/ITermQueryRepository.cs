namespace CharityHub.Core.Contract.Terms.Queries;
using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.Entities;

using GetLastTerm;

public interface ITermQueryRepository : IQueryRepository<Term>
{
    Task<LastTermResponseDto> GetLastTerm(GetLastTermQuery query);
}
