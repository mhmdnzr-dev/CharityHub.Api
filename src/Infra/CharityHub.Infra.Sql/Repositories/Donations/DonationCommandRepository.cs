using CharityHub.Core.Contract.Donations.DTOs.Commands;
using CharityHub.Core.Contract.Donations.Interfaces.Repositories;
using CharityHub.Core.Domain.Entities;
using CharityHub.Infra.Sql.Data.DbContexts;

namespace CharityHub.Infra.Sql.Repositories.Donations;

public class DonationCommandRepository : IDonationCommandRepository
{
    private readonly CharityHubCommandDbContext _dbContext;

    public DonationCommandRepository(CharityHubCommandDbContext dbContext)
    {
        _dbContext = dbContext;
    }



    public async Task AddDonationAsync(CreateDonationCommand donation)
    {
        var newDonation = new Donation
        {
            Id = Guid.NewGuid(),
            Amount = donation.Amount,
            Date = donation.Date
        };

        await _dbContext.Donations.AddAsync(newDonation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateDonationAsync(UpdateDonationCommand donation)
    {
        var existingDonation = await _dbContext.Donations.FindAsync(donation.Id);
        if (existingDonation != null)
        {
            existingDonation.Amount = donation.Amount;
            existingDonation.Date = donation.Date;

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteDonationAsync(DeleteDonationCommand donationId)
    {
        var donation = await _dbContext.Donations.FindAsync(donationId.DonationId);
        if (donation != null)
        {
            _dbContext.Donations.Remove(donation);
            await _dbContext.SaveChangesAsync();
        }
    }
}
