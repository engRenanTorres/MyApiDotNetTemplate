using System.ComponentModel;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services;

public class UserService : IUserService
{
  private readonly ILogger<UserService> _logger;
  private readonly IUserRepository _userRepository;

  public UserService(ILogger<UserService> logger, IUserRepository userRepository)
  {
    _logger = logger;
    _userRepository = userRepository;
  }
  async public Task<bool?> DeleteUser(int id)
  {
    _logger.LogInformation("Delete User Service has been called.");

    User? user = await _userRepository.GetSingleUser(id)
      ?? null;
    if (user == null) return null;

    _userRepository.RemoveEntity<User>(user);

    return await _userRepository.SaveChanges();
  }

  async public Task<IEnumerable<User?>> GetAllUsers()
  {
    _logger.LogInformation("GetUsers Service has been called.");
    var users = await _userRepository.GetAllUsers();
    return users;
  }

  async public Task<User?> GetUser(int id)
  {
    _logger.LogInformation("GetUsers Service has been called.");
    User? user = await _userRepository.GetSingleUser(id);
    return user;
  }
  async public Task<User?> GetUserByEmail(string email)
  {
    _logger.LogInformation("GetUsers Service has been called.");
    User? user = await _userRepository.GetSingleUserByEmail(email);
    return user;
  }

  async public Task<User?> PatchUser(string? userId, [FromBody] UpdateUserDTO updateUserDTO)
  {
    _logger.LogInformation("PatchUsers has been called.");

    if (userId == null) return null;

    User? user = await _userRepository.GetSingleUser(int.Parse(userId));
    if (user == null) return null;
    if (updateUserDTO.Name != null) user.Name = updateUserDTO.Name;
    if (updateUserDTO.Email != null) user.Email = updateUserDTO.Email;

    if (await _userRepository.SaveChanges())
    {
      return user;
    }
    throw new Exception("Error to update User");
  }
}