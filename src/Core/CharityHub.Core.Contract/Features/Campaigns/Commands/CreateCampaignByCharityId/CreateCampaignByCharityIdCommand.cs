namespace CharityHub.Core.Contract.Features.Campaigns.Commands.CreateCampaignByCharityId;

using Primitives.Handlers;

public class CreateCampaignByCharityIdCommand : ICommand<int>
{
    public int CharityId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public int CityId { get; set; }
}