using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotnetAPI.Services;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
  private readonly IUserRepository _userRepository;
  private readonly IAuthService _AuthService;
  private readonly ILogger<AuthController> _logger;
  private readonly AuthHelper _authHelper;
  public AuthController(
    ILogger<AuthController> logger,
    IUserRepository userRepository,
    IConfiguration configuration,
    IAuthService authService
  )
  {
    _userRepository = userRepository;
    _logger = logger;
    _authHelper = new(configuration);
    _AuthService = authService;
  }

  [AllowAnonymous]
  [HttpPost("Resgister")]
  public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
  {
    User? user = await _AuthService.Register(createUserDTO);
    // _constext.SaveChanges return de number of rows that were modified.
    if (user != null)
    {
      return Ok("CRIADO!");
    }

    throw new Exception("Error to Add this User");
  }

  [AllowAnonymous]
  [HttpPost("Login")]
  public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
  {
    var token = await _AuthService.Login(loginDTO);
    if (token == null) return BadRequest("Invalid email or password.");
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });
  }

  [HttpGet("RefreshToken")]
  public IActionResult RefreshToken()
  {
    string? authUserId = User.FindFirst("UserId")?.Value;
    var token = _AuthService.RefreshToken(authUserId);
    if (token == null) return NotFound();
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });
  }
}