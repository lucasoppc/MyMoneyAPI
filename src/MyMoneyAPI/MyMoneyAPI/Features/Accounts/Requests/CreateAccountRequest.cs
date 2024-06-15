using System.Text.Json.Serialization;
using MediatR;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Requests;

public record CreateAccountRequest : IRequest<CreateAccountResponse>
{
    public string Name { get; init; }
    public string BankAccount { get; set; }
    public string Currency { get; init; }
}