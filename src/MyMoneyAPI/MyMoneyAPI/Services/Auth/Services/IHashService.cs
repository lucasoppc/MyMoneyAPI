namespace MyMoneyAPI.Services.Auth.Services;

public interface IHashService
{
    string GenerateHash(string password);
    bool VerifyHash(string password, string hashedPassword);
}