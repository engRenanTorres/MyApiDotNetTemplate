using System.Security.Claims;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
  private readonly IUserRepository _userRepository;
  private readonly ILogger<UserController> _logger;
  private readonly AuthHelper _authHelper;
  public AuthController(
    ILogger<UserController> logger,
    IUserRepository userRepository,
    IConfiguration configuration
  )
  {
    _userRepository = userRepository;
    _logger = logger;
    _authHelper = new(configuration);
  }
  [AllowAnonymous]
  [HttpPost("Resgister")]
  public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
  {
    if (createUserDTO.Email == null) return BadRequest("Email is Obrigatory!");
    User? user = await _userRepository.GetSingleUserByEmail(createUserDTO.Email);
    if (user != null) return NotFound("User already exists.");
    if (createUserDTO.Password == null) return BadRequest("Password is Obrigatory!");
    byte[] passwordHash = _authHelper.PasswordHasher(createUserDTO.Password);
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

  [AllowAnonymous]
  [HttpPost("Login")]
  public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
  {

    User? user = await _userRepository.GetSingleUserByEmail(loginDTO.Email);
    if (user == null) return NotFound("User not found.");
    if (loginDTO.Password == null) return BadRequest("Password is Obrigatory!");
    byte[] inputPasswordHashed = _authHelper.PasswordHasher(loginDTO.Password);
    int passordLength = user.Password.Length;
    if (passordLength != inputPasswordHashed.Length) return Unauthorized("Incorrect password");
    for (int index = 0; index < passordLength; index++)
    {
      if (inputPasswordHashed[index] != user.Password[index])
      {
        return Unauthorized("Incorrect password");
      }
    }
    var token = _authHelper.TokenGenerator(user.Id);
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });

  }

  [HttpGet("RefreshToken")]
  public IActionResult RefreshToken()
  {
    string? authUserId = User.FindFirst("UserId")?.Value;
    if (authUserId == null) return NotFound();
    var token = _authHelper.TokenGenerator(int.Parse(authUserId));
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });
  }
}