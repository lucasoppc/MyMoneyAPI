using MediatR;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Accounts.Requests;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Handlers;

public class GetAccountHandler : IRequestHandler<GetUserAccountsRequest, GetUserAccountsResponse>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<GetUserAccountsResponse> Handle(GetUserAccountsRequest request, CancellationToken cancellationToken = default)
    {
        var response = new GetUserAccountsResponse();
        var accounts = await _accountRepository.GetUserAccountsAsync(request.AccountId);

        foreach (var account in accounts)
        {
            response.UserAccounts.Add(account);
        }

        return response;
    }
}