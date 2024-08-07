using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMoneyAPI.Features.Accounts.Requests;

namespace MyMoneyAPI.Features.Accounts.Controllers.V10;

[ApiController]
[Route("api/v1.0/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var result = await _mediator.Send(request);
        return new OkObjectResult(result);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAccounts()
    {
        var request = new GetUserAccountsRequest();
        var result = await _mediator.Send(request);
        return new OkObjectResult(result);
    }
    
    [HttpDelete("{accountId}")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(string accountId)
    {
        var request = new UpdateAccountRequest
        {
            Id = accountId,
            IsDeleted = true
        };
        var result = await _mediator.Send(request);
        return new OkObjectResult(result);
    }

    [HttpPost("update")]
    [Authorize]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest request)
    {
        var result = await _mediator.Send(request);
        return new OkObjectResult(result);
    }
}