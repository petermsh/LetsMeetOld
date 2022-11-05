using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LetsMeet.API.Services;

public class AuthOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public TimeSpan Expiry { get; set; }
}

public sealed class AuthManager : IAuthManager
{
    private readonly AuthOptions _options;
    private readonly string _issuer;

    public AuthManager(AuthOptions options)
    {
        var issuerSigningKey = options.Key;
        if (issuerSigningKey is null)
        {
            throw new InvalidOperationException("Issuer signing key not set.");
        }

        _options = options;
        _issuer = options.Issuer;
    }

    public string CreateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> { new("id", user.Id.ToString()), new("name", user.UserName) };
        var expires = DateTime.Now.Add(_options.Expiry);

        var token = new JwtSecurityToken(_issuer,
            _issuer,
            claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}