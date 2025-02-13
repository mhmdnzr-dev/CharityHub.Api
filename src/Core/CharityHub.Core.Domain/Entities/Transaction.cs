namespace CharityHub.Core.Domain.Entities;

using Common;

using Identity;

public sealed class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }

    public int UserId { get; private set; }
    public ApplicationUser User { get; private set; }

    public int CampaignId { get; private set; }
    public Campaign Campaign { get; private set; }



    // ** Private constructor for EF Core **
    private Transaction() { }

    // ** Factory Method to Ensure Valid Transactions **
    public static Transaction Create(int userId, int campaignId, decimal amount)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.", nameof(userId));

        if (campaignId <= 0)
            throw new ArgumentException("Invalid campaign ID.", nameof(campaignId));

        if (amount <= 0)
            throw new ArgumentException("Transaction amount must be greater than zero.", nameof(amount));

        return new Transaction
        {
            UserId = userId,
            CampaignId = campaignId,
            Amount = amount
        };
    }
}
