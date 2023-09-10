using DotnetAPI.DTOs;
using DotnetAPI.Enums;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services;
public interface IAuthService
{
  Task<string?> Login([FromBody] LoginDTO loginDTO);
  Task<User?> Register([FromBody] CreateUserDTO createUserDTO);
  string? RefreshToken(string? authUserId, Roles role);
}