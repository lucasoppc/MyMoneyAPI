using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyMoneyAPI.Common.Filter;

public class PasswordFilter : ValidationAttribute
{
    public const string RegexPattern = "^(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+{}\\[\\]:;<>,.?~\\\\/-]).{8,}$";

    public PasswordFilter(string errorMessage)
    :base(errorMessage)
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null) return false;
        if (value is not string str) return false;
        
        return Regex.IsMatch(str, RegexPattern);
    }
}