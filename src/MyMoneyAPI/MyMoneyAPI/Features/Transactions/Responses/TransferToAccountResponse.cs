using MyMoneyAPI.Features.Transactions.Models;

namespace MyMoneyAPI.Features.Transactions.Responses;

public record TransferToAccountResponse(Transaction Transaction)
{
}