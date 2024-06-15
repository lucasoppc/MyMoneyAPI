using MediatR;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Requests;

public class UpdateAccountRequest : IRequest<UpdateAccountResponse>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string BankAccount { get; set; }
    public string Currency { get; set; }
    public bool IsDeleted { get; set; }
}