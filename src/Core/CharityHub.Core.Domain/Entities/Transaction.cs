namespace CharityHub.Core.Domain.Entities;
using CharityHub.Core.Domain.ValueObjects;

// Entity: Transaction
public class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }
    public int UserId { get; private set; }
    public int CampaignId { get; private set; }

    public Transaction(decimal amount, int userId, int campaignId)
    {
        Amount = amount;
        UserId = userId;
        CampaignId = campaignId;
    }
}