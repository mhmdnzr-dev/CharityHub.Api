namespace CharityHub.Core.Domain.Entities;

using Common;

using Enums;

public sealed class Campaign : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal? TotalAmount { get; private set; }
    public decimal? ChargedAmount { get; private set; }
    public int? BannerId { get; private set; }
    public StoredFile? Banner { get; private set; }

    public int CityId { get; private set; }
    public int CharityId { get; private set; }
    public Charity Charity { get; private set; }

    public CampaignStatus CampaignStatus { get; set; }

    private readonly List<Donation> _donations = new();
    public IReadOnlyCollection<Donation> Donations => _donations.AsReadOnly();


    private readonly List<CampaignCategory> _campaignCategories = new();
    public IReadOnlyCollection<CampaignCategory> CampaignCategories => _campaignCategories.AsReadOnly();


    // Private constructor for EF Core
    private Campaign() { }

    // Factory Method for Controlled Creation
    public static Campaign Create(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        decimal? totalAmount,
        int cityId,
        int charityId)
    {
        return new Campaign
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            TotalAmount = totalAmount,
            ChargedAmount = 0,
            CityId = cityId,
            CharityId = charityId,
            CampaignStatus = CampaignStatus.Pending
        };
    }


    public void StartCampaign(DateTime startDate)
    {
        StartDate = startDate;
        CampaignStatus = CampaignStatus.Processing;
    }


    private void EndCampaign(DateTime endDate)
    {
        EndDate = endDate;
        CampaignStatus = ChargedAmount >= TotalAmount ? CampaignStatus.Succeeded : CampaignStatus.Failed;
    }


    public void AddDonation(Donation donation)
    {
        _donations.Add(donation);
        ChargedAmount = ChargedAmount.GetValueOrDefault() + donation.Amount;
    }


    public void AdjustTotalAmount(decimal newTotalAmount)
    {
        if (ChargedAmount > newTotalAmount)
        {
            throw new InvalidOperationException("Total amount cannot be less than the already charged amount.");
        }

        TotalAmount = newTotalAmount;
    }


    public void MarkAsComplete()
    {
        if (ChargedAmount >= TotalAmount)
        {
            EndCampaign(DateTime.UtcNow);
        }
        else
        {
            throw new InvalidOperationException(
                "Campaign cannot be marked as complete until the total amount is reached.");
        }
    }


    public void RemoveDonation(Donation donation)
    {
        if (!_donations.Remove(donation))
        {
            throw new InvalidOperationException("The donation was not found for this campaign.");
        }

        ChargedAmount -= donation.Amount;
    }

    public void SetLogo(string fileName, string filePath, string fileType)
    {
        Banner = new StoredFile(fileName, filePath, fileType);
        BannerId = Banner.Id;
    }
}