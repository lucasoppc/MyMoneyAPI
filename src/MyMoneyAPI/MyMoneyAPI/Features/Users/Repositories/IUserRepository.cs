using Microsoft.Azure.Cosmos;
using MyMoneyAPI.Features.Users.Models;

namespace MyMoneyAPI.Features.Users.Repositories;

public interface IUserRepository
{
    Task<UserEntity> CreateUserAsync(UserEntity user);
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity> GetUserByIdAsync(string userId);
}