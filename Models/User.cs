namespace DotnetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DotnetAPI.Enums;
using Microsoft.EntityFrameworkCore;

[Index(nameof(User.Email), IsUnique = true)]
public class User : Contactable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(name: "Created_at")]
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = "";
    public required byte[] Password { get; set; }
    [JsonIgnore]
    public ICollection<Question>? Questions { get; set; }
    public Roles Role { get; set; } = Roles.User;
}

