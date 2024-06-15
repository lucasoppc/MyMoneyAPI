using MyMoneyAPI.Features.Accounts.Models;

namespace MyMoneyAPI.Features.Accounts.Repositories;

public interface IAccountRepository
{
    Task<Account> CreateAccountAsync(Account account);
    Task<List<Account>> GetUserAccountsAsync(string userId);
    Task<Account> GetUserAccountAsync(string accountId);
    Task SaveAccountAsync(Account account);
}