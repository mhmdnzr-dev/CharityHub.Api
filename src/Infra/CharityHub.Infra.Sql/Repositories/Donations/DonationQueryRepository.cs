using CharityHub.Core.Contract.Donations.Interfaces.Repositories;
using CharityHub.Infra.Sql.Data.DbContexts;


namespace CharityHub.Infra.Sql.Repositories.Donations;


public class DonationQueryRepository : IDonationQueryRepository
{
    private readonly CharityHubQueryDbContext _dbContext;
    public DonationQueryRepository(CharityHubQueryDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    //public async Task<PagedData<GetAllDonationsResponseDto>> GetPagedDonationsAsync(GetAllDonationsQuery query)
    //{
    //    var donationQuery = _dbContext.Donations.AsQueryable();
    //    var totalCount = await donationQuery.CountAsync();

    //    var donations = await donationQuery
    //        .Skip((query.PageNumber - 1) * query.PageSize)
    //        .Take(query.PageSize)
    //        .Select(d => new GetAllDonationsResponseDto
    //        {
    //            Id = d.Id,
    //            Amount = d.Amount,
    //            Date = d.Date
    //        })
    //        .ToListAsync();

    //    return new PagedData<GetAllDonationsResponseDto>
    //    {
    //        Items = donations,
    //        TotalCount = totalCount
    //    };
    //}

    //public async Task<GetDonationByIdResponseDto> GetDonationByIdAsync(GetDonationByIdQuery query)
    //{
    //    var donation = await _dbContext.Donations
    //        .FirstAsync(d => d.Id == query.DonationId);

    //    return new GetDonationByIdResponseDto
    //    {
    //        Id = donation.Id,
    //        Amount = donation.Amount,
    //        Date = donation.Date
    //    };
    //}

}
