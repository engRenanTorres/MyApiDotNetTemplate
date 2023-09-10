using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetAPI.Enums;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
  public class AuthHelper

  {
    private readonly IConfiguration _configuration;
    public AuthHelper(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public byte[] PasswordHasher(string password)
    {
      string salt = _configuration.GetSection("AppSettings:PasswordKey").Value + "Salt";
      return KeyDerivation.Pbkdf2(
        password: password,
        salt: Encoding.ASCII.GetBytes(salt),
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 12,
        numBytesRequested: 256 / 8
      );
    }

    public string TokenGenerator(int userId, Roles roles)
    {
      var claims = new Claim[]{
      new Claim("userId", userId.ToString()),
      new Claim("trampo", roles.ToString())
    };
      string? tokenKeyString = _configuration.GetSection("AppSettings:TokenKey").Value;

      SymmetricSecurityKey tokenKey = new(
              Encoding.UTF8.GetBytes(
                  tokenKeyString ?? ""
              )
          );
      SigningCredentials signingCredentials = new(tokenKey, SecurityAlgorithms.HmacSha256Signature);
      SecurityTokenDescriptor descriptor = new()
      {
        Subject = new ClaimsIdentity(claims, null, nameType: "null", roleType: "role"),
        SigningCredentials = signingCredentials,
        Expires = DateTime.Now.AddDays(1)
      };
      JwtSecurityTokenHandler tokenHandler = new();
      SecurityToken token = tokenHandler.CreateToken(descriptor);
      return tokenHandler.WriteToken(token);
    }
  }

}