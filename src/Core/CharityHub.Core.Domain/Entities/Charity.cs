namespace CharityHub.Core.Domain.Entities;
using System;
using System.Collections.Generic;

using CharityHub.Core.Domain.ValueObjects;

// Aggregate Root: Charity
public class Charity : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Address { get; private set; }
    public string ContactInfo { get; private set; }
    public ICollection<Campaign> Campaigns { get; private set; }

    public Charity(string name, string description, string address, string contactInfo)
    {
        Name = name;
        Description = description;
        Address = address;
        ContactInfo = contactInfo;
        Campaigns = [];
    }

    public Campaign CreateCampaign(string name, decimal goalAmount, DateTime endDate)
    {
        var campaign = new Campaign(name, goalAmount, endDate, this.Id);
        Campaigns.Add(campaign);
        return campaign;
    }
}



