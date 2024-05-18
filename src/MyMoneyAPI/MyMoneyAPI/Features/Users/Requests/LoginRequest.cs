using System.ComponentModel.DataAnnotations;
using MediatR;
using MyMoneyAPI.Features.Users.Responses;

namespace MyMoneyAPI.Features.Users.Requests;

public class LoginRequest : IRequest<LoginResponse>
{
    [Required]
    public string email { get; set; }
    
    [Required]
    public string password { get; set; }
}