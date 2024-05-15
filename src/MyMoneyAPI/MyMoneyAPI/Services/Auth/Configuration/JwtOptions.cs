namespace MyMoneyAPI.Services.Auth.Configuration;

public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
    public int HashIterations { get; set; }
}