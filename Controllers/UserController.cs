using DotnetAPI.DTOs;
using DotnetAPI.Authorization;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[AuthorizationLevel("Staff|Adm")]
public class UserController : ControllerBase
{

  private readonly ILogger<UserController> _logger;
  private readonly IUserService _userService;

  public UserController(
    IUserService userService,
    ILogger<UserController> logger
  )
  {
    _logger = logger;
    _userService = userService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    _logger.LogInformation("GetUsers has been called.");
    User? user = await _userService.GetUser(id);
    if (user == null) return NotFound("User id: " + id + "not found");
    return Ok(user);
  }

  [HttpGet("")]
  public async Task<ActionResult<IEnumerable<User>>> GetUsers()
  {
    _logger.LogInformation("GetUsers has been called.");
    var users = await _userService.GetAllUsers();
    return Ok(users);
  }

  [HttpPatch]
  public async Task<ActionResult<User>> PatchUser([FromBody] UpdateUserDTO updateUserDTO)
  {
    _logger.LogInformation("PatchUsers has been called.");
    string? userId = User?.FindFirst("userId")?.Value;

    if (userId == null) return NotFound("User id: " + userId + "not found");

    User? user = await _userService.PatchUser(userId, updateUserDTO);
    if (user == null) return NotFound("User id: " + userId + "not found");

    return Ok(user);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteUser(int id)
  {
    _logger.LogInformation("DeleteUser has been called.");

    bool? deleteUser = await _userService.DeleteUser(id);

    if (deleteUser == null)
    {
      return NotFound("User id: " + id + "not found");
    }
    if (deleteUser.Value)
    {
      return NoContent();
    };
    throw new Exception("Error to delete User");
  }
}