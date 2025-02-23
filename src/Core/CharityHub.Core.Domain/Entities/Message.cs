namespace CharityHub.Core.Domain.Entities;

using Common;

using Identity;

public class Message : BaseEntity
{
    public int UserId { get; private set; }
    public ApplicationUser User { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsSeen { get; private set; }
    public DateTime? SeenDateTime { get; private set; }

    private Message() { }

 
    // Factory Method to create a new Message
    public static Message Create(int userId,string title, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Message content cannot be empty.", nameof(content));

        return new Message
        {
            UserId = userId,
            Content = content.Trim(),
            Title = content.Trim(),
            IsSeen = false
        };
    }



    public void MarkAsSeen()
    {
        if (!IsSeen)
        {
            IsSeen = true;
            SeenDateTime = DateTime.UtcNow;
        }
    }


    public void UpdateContent(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
            throw new ArgumentException("Message content cannot be empty.", nameof(newContent));

        Content = newContent;
    }


    public bool IsUnread()
    {
        return !IsSeen;
    }


    public void ResetSeenStatus()
    {
        IsSeen = false;
        SeenDateTime = DateTime.MinValue;
    }
}