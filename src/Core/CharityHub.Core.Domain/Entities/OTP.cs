namespace CharityHub.Core.Domain.Entities;

using Enums;

using ValueObjects;

public sealed class OTP : BaseEntity
{
    public int UserId { get; private set; }
    public string OtpCode { get; private set; }
    public OTPStatus Status { get; private set; }
    public string? Description { get; private set; }

    // Constructor to initialize OTP
    private OTP(int userId, string otpCode, OTPStatus status, string? description = null)
    {
        UserId = userId;
        OtpCode = otpCode;
        Status = status;
        Description = description;
    }

    // Factory method to add a new OTP
    public static OTP Add(int userId, string otpCode, string? description = null)
    {
        // OTP is initially in 'Pending' status
        return new OTP(userId, otpCode, OTPStatus.Pending, description);
    }

    // Method to mark OTP as verified (if necessary)
    public void Verify()
    {
        if (Status == OTPStatus.Pending)
        {
            Status = OTPStatus.Verified;
        }
        else
        {
            throw new InvalidOperationException("OTP has already been verified or is not in a valid state.");
        }
    }

    // Method to mark OTP as expired (if necessary)
    public void Expire()
    {
        if (Status == OTPStatus.Pending)
        {
            Status = OTPStatus.Expired;
        }
        else
        {
            throw new InvalidOperationException("OTP has already been processed.");
        }
    }
}
