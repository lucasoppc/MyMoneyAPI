using System.ComponentModel.DataAnnotations;
using MediatR;
using MyMoneyAPI.Common.Filter;
using MyMoneyAPI.Features.Users.Responses;

namespace MyMoneyAPI.Features.Users.Requests;

public record CreateUserRequest : IRequest<CreateUserResponse>
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    
    [Required]
    [PasswordFilter("Password must be at least 8 characters long, contain at least one letter, one number and one special character.")]
    public string Password { get; set;}
    
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [MaxLength(50, ErrorMessage = "Name must be at most 50 characters long.")]
    public string Name { get; set; }
    
    [MaxLength(3, ErrorMessage = "Default currency must be at most 3 characters long.")]
    public string DefaultCurrency { get; set; }
    
}