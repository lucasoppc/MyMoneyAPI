namespace MyMoneyAPI.Common.Constants;

public class CurrencyConstants
{
    public const string USD = "USD";
    public const string UYU = "UYU";
    
    public static bool IsValidCurrency(ref string currency)
    {
        currency = currency.ToUpper();
        return currency is USD or UYU;
    }
}