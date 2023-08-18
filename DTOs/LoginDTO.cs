namespace DotnetAPI.DTOs;

public class LoginDTO
{
  public string Email { get; set; } = "";
  public string Password { get; set; } = "";
  //public byte[] passwordHash { get; set; } = Array.Empty<byte>();
  //public byte[] salt { get; set; } = Array.Empty<byte>();
}