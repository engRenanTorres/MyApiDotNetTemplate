using DotnetAPI.Data;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

  private readonly ILogger<UserController> _logger;
  private readonly IUserRepository _userRepository;

  public UserController(ILogger<UserController> logger, IUserRepository userRepository)
  {
    _logger = logger;
    _userRepository = userRepository;
  }
  [HttpPost("")]
  public async Task<ActionResult<User>> CreateUser(CreateUserDTO userDTO)
  {
    _logger.LogInformation("CreateUser has been called.");

    if (userDTO == null || userDTO.Email == null)
    {
      return BadRequest("User data or email is null.");
    }

    User user = new()

    {
      Name = userDTO.Name,
      Email = userDTO.Email,
      CreatedAt = DateTime.UtcNow
    };


    _userRepository.AddEntity<User>(user);
    // _constext.SaveChanges return de number of rows that were modified.
    if (await _userRepository.SaveChanges())
    {
      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
    throw new Exception("Error to Add this User");

  }

  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    _logger.LogInformation("GetUsers has been called.");
    User? user = await _userRepository.GetSingleUser(id);

    return Ok(user);

  }

  [HttpGet("")]
  public async Task<ActionResult<IEnumerable<User>>> GetUsers()
  {
    _logger.LogInformation("GetUsers has been called.");
    var users = await _userRepository.GetAllUsers();
    return Ok(users);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<User>> PatchUser(int id, [FromBody] UpdateUserDTO updateUserDTO)
  {
    _logger.LogInformation("PatchUsers has been called.");

    User? user = await _userRepository.GetSingleUser(id);
    if (user == null)
      if (user == null)
      {
        return NotFound("User id: " + id + "not found");
      }
    if (updateUserDTO.Name != null) user.Name = updateUserDTO.Name;
    if (updateUserDTO.Email != null) user.Email = updateUserDTO.Email;

    if (await _userRepository.SaveChanges())
    {
      return Ok(user);
    }
    throw new Exception("Error to update User");

  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteUser(int id)
  {
    _logger.LogInformation("DeleteUser has been called.");

    User? user = await _userRepository.GetSingleUser(id);

    if (user == null)
    {
      return NotFound("User id: " + id + "not found");
    }

    _userRepository.RemoveEntity<User>(user);
    if (await _userRepository.SaveChanges())
    {
      return NoContent();
    };
    throw new Exception("Error to delete User");

  }


}