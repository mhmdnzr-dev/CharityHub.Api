namespace CharityHub.Core.Domain.ValueObjects;


public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public bool IsActive { get; set; } = true;
}


public abstract class Aggregate<TAggregate> : BaseEntity
    where TAggregate : BaseEntity
{
    public TAggregate AggregateRoot { get; protected set; }
}