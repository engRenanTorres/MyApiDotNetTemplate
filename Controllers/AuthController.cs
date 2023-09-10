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
  private readonly IUserService _userService;
  private readonly IAuthService _AuthService;
  private readonly ILogger<AuthController> _logger;
  private readonly AuthHelper _authHelper;
  public AuthController(
    ILogger<AuthController> logger,
    IUserService userService,
    IConfiguration configuration,
    IAuthService authService
  )
  {
    _userService = userService;
    _logger = logger;
    _authHelper = new(configuration);
    _AuthService = authService;
  }

  [AllowAnonymous]
  [HttpPost("Resgister")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  public async Task<ActionResult<User>> Register([FromBody] CreateUserDTO createUserDTO)
  {
    User? user = await _AuthService.Register(createUserDTO);
    // _constext.SaveChanges return de number of rows that were modified.
    if (user != null)
    {
      return Created("CRIADO!", user);
    }

    throw new Exception("Error to Add this User");
  }

  [AllowAnonymous]
  [HttpPost("Login")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<Dictionary<string, string>>> Login([FromBody] LoginDTO loginDTO)
  {
    var token = await _AuthService.Login(loginDTO);
    if (token == null) return BadRequest("Invalid email or password.");
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });
  }

  [HttpGet("RefreshToken")]
  public async Task<IActionResult> RefreshToken()
  {
    string? authUserId = User.FindFirst("UserId")?.Value;

    if (authUserId == null) return BadRequest("UserId cannot be converted into int type");
    User? user = await _userService.GetUser(int.Parse(authUserId));
    if (user == null) return BadRequest("User does not exist");

    var token = _AuthService.RefreshToken(authUserId, user.Role);
    if (token == null) return NotFound();
    return Ok(new Dictionary<string, string>{
      {"token", token},
    });
  }
}