using MyMoneyAPI.Features.Accounts.Models;

namespace MyMoneyAPI.Features.Accounts.Responses;

public class GetUserAccountsResponse
{
    public List<Account> UserAccounts { get; } = new List<Account>();
}