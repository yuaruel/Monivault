using System.Text.RegularExpressions;
using Abp.Extensions;

namespace Monivault.Validation
{
    public static class ValidationHelper
    {
        public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public const string PhoneRegex = @"\d{11}";

        public static bool IsEmail(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }

            var regex = new Regex(EmailRegex);
            return regex.IsMatch(value);
        }

        public static bool IsPhoneNumber(string phone)
        {
            if (phone.IsNullOrEmpty()) return false;

            var regex = new Regex(PhoneRegex);
            return regex.IsMatch(phone);
        }
    }
}
