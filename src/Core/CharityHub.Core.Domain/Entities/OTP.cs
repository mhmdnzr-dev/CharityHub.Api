namespace CharityHub.Core.Domain.Entities;

using Enums;

using ValueObjects;

public sealed class OTP : BaseEntity
{
    public int UserId { get; private set; }
    public string OtpCode { get; private set; }
    public OTPStatus Status { get; private set; }
    public string? Description { get; private set; }
}
