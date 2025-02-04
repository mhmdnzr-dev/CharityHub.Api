using CharityHub.Core.Domain.Entities;

namespace CharityHub.Core.Domain.ValueObjects;


public class CharityCategory
{
    public int CharityId { get; private set; }
    public virtual Category Category { get; private set; }


    public int CategoryId { get; private set; }
    public virtual Charity Charity { get; private set; }
}