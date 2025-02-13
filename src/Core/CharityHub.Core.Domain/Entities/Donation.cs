namespace CharityHub.Core.Domain.Entities;

using Common;

using Identity;

public sealed class Donation : BaseEntity
{
    public int UserId { get; private set; }
    public ApplicationUser User { get; private set; }
    public int CampaignId { get; private set; }
    public Campaign Campaign { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DonatedAt { get; private set; }

    // ** Private constructor for EF Core **
    private Donation() { }

    // ** Factory Method to Ensure Valid Donations **
    public static Donation Create(int userId, int campaignId, decimal amount)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.", nameof(userId));

        if (campaignId <= 0)
            throw new ArgumentException("Invalid campaign ID.", nameof(campaignId));

        if (amount <= 0)
            throw new ArgumentException("Donation amount must be greater than zero.", nameof(amount));

        return new Donation
        {
            UserId = userId,
            CampaignId = campaignId,
            Amount = amount,
            DonatedAt = DateTime.UtcNow // Auto-set donation time
        };
    }
}
