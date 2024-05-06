
namespace MyMoneyAPI.Features.Accounts.Responses;

public record CreateAccountResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Currency { get; init; }
}