using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyMoneyAPI.Common.Filter;

public class PasswordFilter : Attribute
{
    private const string regexPattern = "^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{8,}$\n";
    public string ErrorMessage { get; }

    public PasswordFilter(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public bool IsValid(string value)
    {
        return Regex.IsMatch(value, regexPattern);
    }
}