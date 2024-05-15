namespace MyMoneyAPI.Features.Users.Responses;

public record CreateUserResponse(string UserId, string Name, string Email, string DefaultCurrency, string Token);