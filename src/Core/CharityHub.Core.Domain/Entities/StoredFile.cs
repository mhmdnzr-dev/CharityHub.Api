namespace CharityHub.Core.Domain.Entities;

using ValueObjects;

public sealed class StoredFile : BaseEntity
{
    public string FileName { get; set; } = string.Empty; // e.g., "logo.png"
    public string FilePath { get; set; } = string.Empty; // e.g., "/uploads/charities/logo.png"
    public string FileType { get; set; } = string.Empty; // e.g., "image/png"
}
