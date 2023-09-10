using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Helpers;
using DotnetAPI.Data.Repositories;
using DotnetAPI.Enums;

namespace DotnetAPI.Services;

public class AuthService : IAuthService
{
  private readonly IUserService _userService;
  private readonly ILogger<AuthService> _logger;
  private readonly AuthHelper _authHelper;
  private readonly IUserRepository _userRepository;

  public AuthService(
    ILogger<AuthService> logger,
    IUserService userService,
    IConfiguration configuration,
    IUserRepository userRepository
  )
  {
    _userService = userService;
    _logger = logger;
    _authHelper = new(configuration);
    _userRepository = userRepository;
  }

  async public Task<string?> Login([FromBody] LoginDTO loginDTO)
  {
    User? user = await _userService.GetUserByEmail(loginDTO.Email);

    if (loginDTO.Password == null || user == null) return null;
    byte[] inputPasswordHashed = _authHelper.PasswordHasher(loginDTO.Password);
    int passordLength = user.Password.Length;
    if (passordLength != inputPasswordHashed.Length) return null;
    for (int index = 0; index < passordLength; index++)
    {
      if (inputPasswordHashed[index] != user.Password[index])
      {
        return null;
      }
    }
    var token = _authHelper.TokenGenerator(user.Id, user.Role);
    return token;
  }

  public string? RefreshToken(string? authUserId, Roles role)
  {
    if (authUserId == null) return null;
    var token = _authHelper.TokenGenerator(int.Parse(authUserId), role);
    return token;
  }

  async public Task<User?> Register([FromBody] CreateUserDTO createUserDTO)
  {
    if (createUserDTO.Email == null) throw new Exception("Email is Obrigatory!");
    if (createUserDTO.Password == null) throw new Exception("Password is Obrigatory!");
    User? user = await _userRepository.GetSingleUserByEmail(createUserDTO.Email);
    if (user != null) throw new Exception("User already exists.");
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
      return user;
    }
    throw new Exception("Error to Add this User");
  }
}