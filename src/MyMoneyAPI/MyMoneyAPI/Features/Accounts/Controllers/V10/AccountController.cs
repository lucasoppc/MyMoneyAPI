using System.Net;
using MediatR;
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
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var result = await _mediator.Send(request);
        return new OkObjectResult(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] string accountId)
    {
        var request = new GetUserAccountsRequest(accountId);
        var result = await _mediator.Send(request);
        return new OkObjectResult(result);
    }
}