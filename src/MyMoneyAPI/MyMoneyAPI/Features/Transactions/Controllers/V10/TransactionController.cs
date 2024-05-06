using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMoneyAPI.Features.Transactions.Requests;

namespace MyMoneyAPI.Features.Transactions.Controllers.V10;

[ApiController]
[Route("api/V1.0/Transactions")]
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
        return new CreatedResult();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrnsactions([FromQuery] GetTransactions request)
    {
        var response = await _mediator.Send(request);
        return new OkObjectResult(response);
    }
}