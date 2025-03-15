namespace CharityHub.Core.Contract.Features.Transactions.Queries.GetUserTransactions;

public class UserTransactionsResponseDto
{
    public string CampaignName { get; set; }
    public string CharityName { get; set; }
    public decimal AmountOfAssistance { get; set; }
    public DateTime AssistanceDate { get; set; }
    public string CampaignBannerPhotoUrlAddress { get; set; }
    public string CharityLogoUrl { get; set; }
    public int CharityId { get; set; }
}