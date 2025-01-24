

namespace CharityHub.Core.Domain.Entities;

using System.ComponentModel.DataAnnotations;

using ValueObjects;

public sealed class Category : BaseEntity
{
    [StringLength(10)]
    public string Name { get; private set; }
}