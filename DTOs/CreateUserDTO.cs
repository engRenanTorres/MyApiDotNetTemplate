using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.DTOs;
public class CreateUserDTO
{
    [Required]
    public string Name { get; set; } = "";
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    public string? SocialMedia { get; set; }
    public string? Website { get; set; }
}
