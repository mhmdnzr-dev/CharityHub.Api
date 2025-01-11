
using CharityHub.Core.Contract.Donations.Queries.GetAllDonations;
using CharityHub.Core.Contract.Primitives.Models;
using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.Entities;

namespace CharityHub.Core.Contract.Donations.Queries;

public interface IDonationQueryRepository : IQueryRepository<Donation>
{
    Task<PagedData<GetAllDonationsResponseDto>> GetPagedDonationsAsync(GetAllDonationsQuery query);
}
