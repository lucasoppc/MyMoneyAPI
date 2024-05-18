using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Features.Users.Models;

namespace MyMoneyAPI.Features.Users.Repositories;

public interface IUserRepository
{
    Task<UserEntity> CreateUserAsync(UserEntity user, CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserEntity> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
}