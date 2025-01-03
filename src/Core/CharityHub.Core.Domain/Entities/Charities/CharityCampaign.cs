namespace CharityHub.Core.Domain.Entities.Charities;
using System;

using CharityHub.Core.Domain.ValueObjects;

public class CharityCampaign : Aggregate<Charity>
{
    public string Name { get; set; }
    public string Details { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CollectedAmount { get; private set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }

    public void Donate(decimal amount)
    {
        CollectedAmount += amount;
    }

    public bool IsCompleted()
    {
        return CollectedAmount >= TargetAmount || DateTime.UtcNow > EndDate;
    }
}