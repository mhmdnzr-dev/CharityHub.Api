namespace CharityHub.Core.Contract.Messages.Queries.GetMessageByUserIdQuery;

public class MessageByUserIdDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsSeen { get; set; }
    public DateTime? SeenDate { get; set; }
}