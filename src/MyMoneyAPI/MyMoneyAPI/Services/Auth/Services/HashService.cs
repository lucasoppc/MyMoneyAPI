using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using MyMoneyAPI.Services.Auth.Configuration;

namespace MyMoneyAPI.Services.Auth.Services;

public class HashService : IHashService
{
    private readonly int _pbkdf2IterCount; // non-default for Rfc2898DeriveBytes
    private const int Pbkdf2SubkeyLength = 512 / 8; // 256 bits
    private const int SaltSize = 256 / 8; // 128 bits


    public HashService(IOptions<JwtOptions> options)
    {
        _pbkdf2IterCount = options.Value.HashIterations;
    }

    public string GenerateHash(string password)
    {
        var salt = new byte[SaltSize];
        byte[] hash;

        RandomNumberGenerator.Create().GetBytes(salt);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _pbkdf2IterCount))
        {
            hash = pbkdf2.GetBytes(Pbkdf2SubkeyLength);
        }

        var hashBytes = new byte[Pbkdf2SubkeyLength + SaltSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, Pbkdf2SubkeyLength);

        return Convert.ToBase64String(hashBytes);
    }
    
    public bool VerifyHash(string password, string hashedPassword)
    {
        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        if (hashedPasswordBytes.Length != (SaltSize + Pbkdf2SubkeyLength))
        {
            return false;
        }

        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        byte[] hash;

        Array.Copy(hashBytes, 0, salt, 0, SaltSize);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _pbkdf2IterCount))
        {
            hash = pbkdf2.GetBytes(Pbkdf2SubkeyLength);
        }
        for (var i = 0; i < Pbkdf2SubkeyLength; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }
        return true;
    }
}