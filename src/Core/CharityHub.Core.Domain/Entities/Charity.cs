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

    public int? LogoId { get; private set; }
    public StoredFile? Logo { get; private set; }

    public int? BannerId { get; private set; }
    public StoredFile? Banner { get; private set; }

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

    public void SetBanner(string fileName, string filePath, string fileType)
    {
        Banner = new StoredFile(fileName, filePath, fileType);
        BannerId = Banner.Id;
    }

    public void SetLogo(string fileName, string filePath, string fileType)
    {
        Logo = new StoredFile(fileName, filePath, fileType);
        LogoId = Logo.Id;
    }
    
    public void SetCampaign(Campaign campaign)
    {
        if (_campaigns.Any(c => c.Id == campaign.Id))
        {
            throw new InvalidOperationException("This campaign is already associated with the charity.");
        }

        if (campaign.CharityId != this.Id)
        {
            throw new InvalidOperationException("The campaign does not belong to this charity.");
        }

        _campaigns.Add(campaign);
    }
    
    public void RemoveCampaignById(int campaignId)
    {
        var campaign = _campaigns.SingleOrDefault(c => c.Id == campaignId);

        if (campaign == null)
        {
            throw new InvalidOperationException("The campaign with the given ID was not found for this charity.");
        }

        _campaigns.Remove(campaign);
    }
}