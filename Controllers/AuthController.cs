using System.Security.Claims;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IUserRepository _userRepository;
  private readonly ILogger<UserController> _logger;
  private readonly IConfiguration _configuration;
  public AuthController(ILogger<UserController> logger, IUserRepository userRepository, IConfiguration configuration)
  {
    _userRepository = userRepository;
    _logger = logger;
    _configuration = configuration;
  }
  [HttpPost("Resgister")]
  public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
  {
    if (createUserDTO.Email == null) return BadRequest("Email is Obrigatory!");
    User? user = await _userRepository.GetSingleUserByEmail(createUserDTO.Email);
    if (user != null) return NotFound("User already exists.");
    string passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + "Salt";
    if (createUserDTO.Password == null) return BadRequest("Password is Obrigatory!");
    byte[] passwordHash = PasswordHasher(createUserDTO.Password, passwordSaltPlusString);
    user = new()

    {
      Name = createUserDTO.Name,
      Email = createUserDTO.Email,
      Password = passwordHash,
      CreatedAt = DateTime.UtcNow
    };


    _userRepository.AddEntity<User>(user);
    // _constext.SaveChanges return de number of rows that were modified.
    if (await _userRepository.SaveChanges())
    {
      return Ok("CRIADO!");
    }
    throw new Exception("Error to Add this User");
  }

  [HttpPost("Login")]
  public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
  {

    User? user = await _userRepository.GetSingleUserByEmail(loginDTO.Email);
    if (user == null) return NotFound("User not found.");
    string passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + "Salt";
    if (loginDTO.Password == null) return BadRequest("Password is Obrigatory!");
    byte[] inputPasswordHashed = PasswordHasher(loginDTO.Password, passwordSaltPlusString);
    int passordLength = user.Password.Length;
    if (passordLength != inputPasswordHashed.Length) return Unauthorized("Incorrect password");
    for (int index = 0; index < passordLength; index++)
    {
      if (inputPasswordHashed[index] != user.Password[index])
      {
        return Unauthorized("Incorrect password");
      }
    }
    var token = TokenGenerator(user.Id);
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });

  }

  private static byte[] PasswordHasher(string password, string salt)
  {
    return KeyDerivation.Pbkdf2(
      password: password,
      salt: Encoding.ASCII.GetBytes(salt),
      prf: KeyDerivationPrf.HMACSHA256,
      iterationCount: 12,
      numBytesRequested: 256 / 8
    );
  }

  private string TokenGenerator(int userId)
  {
    var claims = new Claim[]{
      new Claim("userId", userId.ToString())
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
      Subject = new ClaimsIdentity(claims),
      SigningCredentials = signingCredentials,
      Expires = DateTime.Now.AddDays(1)
    };
    JwtSecurityTokenHandler tokenHandler = new();
    SecurityToken token = tokenHandler.CreateToken(descriptor);
    return tokenHandler.WriteToken(token);
  }

  /*private bool ValidateToken (string token)
  {
    string? tokenKeyString = builder.Configuration.GetSection("AppSettings:Token").Value;
     
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters() 
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        tokenKeyString != null ? tokenKeyString : ""
                    )),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
  }*/
}