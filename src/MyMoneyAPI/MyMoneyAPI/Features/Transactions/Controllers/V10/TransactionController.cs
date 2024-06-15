using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMoneyAPI.Features.Transactions.Requests;

namespace MyMoneyAPI.Features.Transactions.Controllers.V10;

[ApiController]
[Route("api/V1.0/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        var response = await _mediator.Send(request);
        return new OkObjectResult(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] GetTransactions request)
    {
        var response = await _mediator.Send(request);
        return new OkObjectResult(response);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferToAccount([FromBody] TransferToAccountRequest transferToAccountRequest)
    {
        var response = await _mediator.Send(transferToAccountRequest);
        return new OkObjectResult(response);
    }
}