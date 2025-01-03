namespace CharityHub.Core.Domain.Entities;
using System;
using System.Collections.Generic;

using CharityHub.Core.Domain.ValueObjects;

// Aggregate Root: Campaign
public class Campaign : BaseEntity
{
    public string Name { get; private set; }
    public decimal GoalAmount { get; private set; }
    public decimal CollectedAmount { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int CharityId { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; }

    public Campaign(string name, decimal goalAmount, DateTime endDate, int charityId)
    {
        Name = name;
        GoalAmount = goalAmount;
        CollectedAmount = 0;
        StartDate = DateTime.UtcNow;
        EndDate = endDate;
        CharityId = charityId;
        Transactions = [];
    }

    public void RecordTransaction(decimal amount, int userId)
    {
        var transaction = new Transaction(amount, userId, this.Id);
        Transactions.Add(transaction);
        CollectedAmount += amount;
    }
}