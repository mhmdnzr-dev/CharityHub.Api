namespace CharityHub.Core.Domain.Entities;

using ValueObjects;



public sealed class Campaign : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public decimal? TotalAmount { get; private set; }
    public decimal? ChargedAmount { get; private set; }
    public int PhotoId { get; private set; }
    public int CityId { get; private set; }
    public int CharityId { get; private set; }
    public Charity Charity { get; private set; }
    

    
    private readonly List<Donation> _donations = new();
    public IReadOnlyCollection<Donation> Donations => _donations.AsReadOnly();

    // ** Private constructor for EF Core **
    private Campaign() { }

    // ** Factory Method for Controlled Creation **
    public static Campaign Create(
        string title,
        string description,
        DateTime? startDate,
        DateTime? endDate,
        decimal? totalAmount,
        int photoId,
        int cityId,
        int charityId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (totalAmount < 0)
            throw new ArgumentException("Total amount cannot be negative.", nameof(totalAmount));

        return new Campaign
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            TotalAmount = totalAmount,
            ChargedAmount = 0, // Default initial charged amount
            PhotoId = photoId,
            CityId = cityId,
            CharityId = charityId
        };
    }

    // ** Business Logic: Add Donation to Campaign **
    public void AddDonation(Donation donation)
    {
        if (donation is null)
            throw new ArgumentNullException(nameof(donation));

        if (donation.Amount <= 0)
            throw new InvalidOperationException("Donation amount must be greater than zero.");

        _donations.Add(donation);
        ChargedAmount += donation.Amount;
    }

   
}
