namespace CharityHub.Utils.Extensions.Extensions;

using System.Text.RegularExpressions;

public class PhoneNumberHelpers
{
    private static readonly Regex IranianMobileNumberRegex = new Regex(@"^(\+98|0|098)?9\d{9}$", RegexOptions.Compiled);

    public static bool IsValidIranianMobileNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }
        return IranianMobileNumberRegex.IsMatch(phoneNumber);
    }
}
