namespace CharityHub.Core.Contract.Features.Messages.Queries.GetMessageByUserIdQuery;

public class MessageByUserIdDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
    public bool IsSeen { get; set; }
    public DateTime? SeenDate { get; set; }
}