using CharityHub.Core.Contract.Donations.Commands;
using CharityHub.Core.Domain.Entities;
using CharityHub.Infra.Sql.Data.DbContexts;
using CharityHub.Infra.Sql.Premitives;

namespace CharityHub.Infra.Sql.Repositories.Donations;

public class DonationCommandRepository(CharityHubCommandDbContext commandDbContext) : CommandRepository<Donation>(commandDbContext), IDonationCommandRepository
{



    //public async Task AddDonationAsync(CreateDonationCommand donation)
    //{
    //    var newDonation = new Donation
    //    {
    //        Id = Guid.NewGuid(),
    //        Amount = donation.Amount,
    //        Date = donation.Date
    //    };

    //    await _dbContext.Donations.AddAsync(newDonation);
    //    await _dbContext.SaveChangesAsync();
    //}

    //public async Task UpdateDonationAsync(UpdateDonationCommand donation)
    //{
    //    var existingDonation = await _dbContext.Donations.FindAsync(donation.Id);
    //    if (existingDonation != null)
    //    {
    //        existingDonation.Amount = donation.Amount;
    //        existingDonation.Date = donation.Date;

    //        await _dbContext.SaveChangesAsync();
    //    }
    //}

    //public async Task DeleteDonationAsync(DeleteDonationCommand donationId)
    //{
    //    var donation = await _dbContext.Donations.FindAsync(donationId.DonationId);
    //    if (donation != null)
    //    {
    //        _dbContext.Donations.Remove(donation);
    //        await _dbContext.SaveChangesAsync();
    //    }
    //}
}
