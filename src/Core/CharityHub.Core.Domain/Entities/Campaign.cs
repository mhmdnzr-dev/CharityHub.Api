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

    public int? CityId { get; private set; }

    public int CharityId { get; private set; }
    public Charity Charity { get; private set; }
    public ICollection<Donation> Donations { get; private set; } = new HashSet<Donation>();
}