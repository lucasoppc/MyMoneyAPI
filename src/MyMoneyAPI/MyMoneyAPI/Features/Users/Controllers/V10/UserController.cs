using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMoneyAPI.Features.Users.Requests;

namespace MyMoneyAPI.Features.Users.Controllers.V10;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
}