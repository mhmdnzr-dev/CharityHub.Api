namespace CharityHub.Core.Contract.Charity.Queries.GetAllCharities;

public class AllCharitiesResponseDto
{
    public int Id { get; set; }
    public string LogoUriAddress { get; set; }
    public string Name { get; set; }
    public int CampaignCount { get; set; }
}