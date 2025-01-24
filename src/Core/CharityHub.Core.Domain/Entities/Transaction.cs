namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public sealed class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }

    public int UserId { get; private set; }
    public ApplicationUser User { get; private set; }
    public int CampaignId { get; private set; }
    public Campaign Campaign { get; private set; }
}