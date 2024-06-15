using System.ComponentModel.DataAnnotations;
using MediatR;
using MyMoneyAPI.Features.Transactions.Responses;

namespace MyMoneyAPI.Features.Transactions.Requests;

public class TransferToAccountRequest : IRequest<TransferToAccountResponse>
{
    [Required]
    public string FromAccountId { get; set; }
    
    [Required]
    public string ToAccountId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
}