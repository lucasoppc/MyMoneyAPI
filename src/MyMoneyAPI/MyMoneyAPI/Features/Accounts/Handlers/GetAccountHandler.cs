using MediatR;
using MyMoneyAPI.Features.Accounts.Repositories;
using MyMoneyAPI.Features.Accounts.Requests;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Handlers;

public class GetAccountHandler(IAccountRepository accountRepository,
    IHttpContextAccessor contextAccessor)
    : IRequestHandler<GetUserAccountsRequest, GetUserAccountsResponse>
{
    
    public async Task<GetUserAccountsResponse> Handle(GetUserAccountsRequest request, CancellationToken cancellationToken = default)
    {
        var response = new GetUserAccountsResponse();
        
        var userId = contextAccessor?.HttpContext?.User.FindFirst("UserId")?.Value;
        var accounts = await accountRepository.GetUserAccountsAsync(userId);

        foreach (var account in accounts)
        {
            response.UserAccounts.Add(account);
        }

        return response;
    }
}