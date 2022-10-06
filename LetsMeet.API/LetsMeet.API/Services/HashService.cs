using System.Security.Cryptography;
using LetsMeet.API.Interfaces;

namespace LetsMeet.API.Services;

internal class HashService : IHashService
{
    private const string peper = "6EEB576B3ADB43A6A52AB37461E433F5"; //32
    private const int saltSize = 32;
    private const int hashSize = 64;
    private const int iterations = 10;

    public string Hash(string password)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltSize]);
        return Hash(password, salt);
    }

    private string Hash(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes($"{password}{peper}", salt, iterations);

        var hash = pbkdf2.GetBytes(hashSize + 32);
        return $"{Convert.ToBase64String(salt)};{Convert.ToBase64String(hash)}";
    }

    public bool Check(string hashed, string password)
    {
        var salt = Convert.FromBase64String(hashed.Split(';')[0]);
        return Hash(password, salt) == hashed;
    }
}