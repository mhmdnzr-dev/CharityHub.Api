

namespace CharityHub.Core.Domain.Entities;

using Identity;

using ValueObjects;

public sealed class Charity : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Website { get; private set; }

    public int CreatedByUserId { get; private set; }
    public ApplicationUser ApplicationUser { get; private set; }

    public string Address { get; private set; }
    public int? CityId { get; private set; }
    public string Telephone { get; private set; }
    public string ManagerName { get; private set; }

    public int? LogoId { get; set; }
    public StoredFile? Logo { get; set; }

    public int? BannerId { get; set; }
    public StoredFile? Banner { get; set; }

    public int SocialId { get; private set; }
    public Social Social { get; private set; }

    public string ContactName { get; private set; }
    public string ContactPhone { get; private set; }

    private readonly List<Campaign> _campaigns = new();
    public IReadOnlyCollection<Campaign> Campaigns => _campaigns.AsReadOnly();

    // ** Private constructor for EF Core **
    private Charity() { }

    // ** Factory Method to Ensure Valid Charities **
    public static Charity Create(
        string name,
        string description,
        string website,
        int createdByUserId,
        string address,
        int? cityId,
        string telephone,
        string managerName,
   
        int socialId,
        string contactName,
        string contactPhone)
    {


        return new Charity
        {
            Name = name.Trim(),
            Description = description.Trim(),
            Website = website.Trim(),
            CreatedByUserId = createdByUserId,
            Address = address.Trim(),
            CityId = cityId,
            Telephone = telephone.Trim(),
            ManagerName = managerName.Trim(),
            SocialId = socialId,
            ContactName = contactName.Trim(),
            ContactPhone = contactPhone.Trim()
        };
    }

    // ** Method to Add a Campaign to the Charity **
    public void AddCampaign(Campaign campaign)
    {
        if (campaign == null)
            throw new ArgumentNullException(nameof(campaign), "Campaign cannot be null.");

        _campaigns.Add(campaign);
    }
}
