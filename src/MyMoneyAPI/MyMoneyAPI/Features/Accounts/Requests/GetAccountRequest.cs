using MediatR;
using MyMoneyAPI.Features.Accounts.Responses;

namespace MyMoneyAPI.Features.Accounts.Requests;

public record GetUserAccountsRequest() : IRequest<GetUserAccountsResponse>;