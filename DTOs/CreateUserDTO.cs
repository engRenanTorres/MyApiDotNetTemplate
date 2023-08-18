namespace DotnetAPI.DTOs;
public class CreateUserDTO
{
    public string Name { get; set; } = "";
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? SocialMedia { get; set; }
    public string? Website { get; set; }
}
