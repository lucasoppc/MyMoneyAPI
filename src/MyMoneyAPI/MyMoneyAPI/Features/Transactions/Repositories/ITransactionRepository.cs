using MyMoneyAPI.Features.Transactions.Models;
using MyMoneyAPI.Features.Transactions.Requests;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> CreateTransactionAsync(Transaction transaction);
    Task<List<Transaction>> GetTransactionsAsync(string accountId);
}