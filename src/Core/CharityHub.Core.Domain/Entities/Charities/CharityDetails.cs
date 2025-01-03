namespace CharityHub.Core.Domain.Entities.Charities;

using CharityHub.Core.Domain.ValueObjects;

public class CharityDetails : Aggregate<Charity>
{
    public string Description { get; set; }
    public string Website { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
}