namespace MyMoneyAPI.Features.Users.Models;

public class UserEntity
{
    public string id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string passwordHash { get; set; }
    public string defaultCurrency { get; set; }
}