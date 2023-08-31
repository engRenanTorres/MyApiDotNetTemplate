using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services
{
  public interface IUserService
  {
    Task<User?> GetUser(int id);
    Task<User?> GetUserByEmail(string email);
    Task<IEnumerable<User?>> GetAllUsers();
    Task<User?> PatchUser(string? userId, [FromBody] UpdateUserDTO updateUserDTO);
    Task<bool?> DeleteUser(int id);
  }
}