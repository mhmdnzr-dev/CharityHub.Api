namespace CharityHub.Core.Contract.Transactions.Queries.GetUserTransactions;

public class UserTransactionsResponseDto
{
    public string CampaignName { get; set; }
    public string CharityName { get; set; }
    public decimal AmountOfAssistance { get; set; }
    public DateTime AssistanceDate { get; set; }  
}